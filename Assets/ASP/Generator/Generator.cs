using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASP
{
    public class Generator : MonoBehaviour
    {
        [SerializeField] protected int cpus = 4, timeout = 100;
        [SerializeField] protected Clingo.ClingoSolver solver;

        protected bool waitingOnClingo;

        protected System.Action<Clingo.AnswerSet, string> satifiableCallBack;
        protected System.Action<string> unsatifiableCallBack;
        protected System.Action<int, string> timedoutCallBack;
        protected System.Action<string, string> errorCallBack;

        protected string filename;
        protected string jobID = "";

        [SerializeField] bool runOnStart = false;
        private void Start()
        {
            if (runOnStart)
            {
                InitializeGenerator(SATISFIABLE, UNSATISFIABLE, TIMEDOUT, ERROR);
                startGenerator();
            }

        }
        // Update is called once per frame
        void Update()
        {
            if (waitingOnClingo)
            {
                if (solver.SolverStatus == Clingo.ClingoSolver.Status.SATISFIABLE)
                {
                    finalizeGenerator();
                    FindObjectOfType<DebugMap>().DisplayMap(solver.answerSet);
                    FindObjectOfType<DebugMap>().AdjustCamera();
                    satifiableCallBack(solver.answerSet, jobID);
                }
                else if (solver.SolverStatus == Clingo.ClingoSolver.Status.UNSATISFIABLE)
                {
                    finalizeGenerator();
                    unsatifiableCallBack(jobID);
                }
                else if (solver.SolverStatus == Clingo.ClingoSolver.Status.ERROR)
                {
                    finalizeGenerator();
                    errorCallBack(solver.ClingoConsoleError, jobID);
                }
                else if (solver.SolverStatus == Clingo.ClingoSolver.Status.TIMEDOUT)
                {
                    finalizeGenerator();
                    timedoutCallBack(timeout, jobID);
                }
            }
        }


        public void StartGenerator(string jobID)
        {
            this.jobID = jobID;
            initializeGenerator();
            startGenerator();
        }

        private string aspCode { get { return getASPCode(); } }

        virtual protected string getASPCode()
        {
            string aspCode = @"

                #const max_width = 50.
                #const max_height = 50.

                width(1..max_width).
                height(1..max_height).

                tile_type(filled;empty).

                1{tile(XX, YY, Type): tile_type(Type)}1 :- width(XX), height(YY).

                %:- Count = {tile(_,_,filled)}, Count != max_width.

                %:- tile(X1,Y1, filled), tile(X2,Y2,filled), X1 == X2, Y1 != Y2.
                %:- tile(X1,Y1, filled), tile(X2,Y2,filled), Y1 == Y2, X1 != X2.
                
                %:- tile(X1,Y1,filled), tile(X2,Y2,filled), Delta = (X1 - X2) - (Y1 - Y2), Delta == 0, Y1 != Y2, X1 != X2.
                %:- tile(X1,Y1,filled), tile(X2,Y2,filled), D1 = X1 - X2, D2 = Y1 - Y2, D1 == -D2, Y1 != Y2.

                %:- tile(XX,YY,filled), tile(XX + Offset, YY + Offset,filled), Offset = (1..max_width).
                %:- tile(XX,YY,filled), tile(XX + Offset, YY - Offset,filled), Offset = (1..max_width).
            ";

            return aspCode;
        }
        public void InitializeGenerator(System.Action<Clingo.AnswerSet, string> satifiableCallBack, System.Action<string> unsatifiableCallBack, System.Action<int, string> timedoutCallBack, System.Action<string, string> errorCallBack)
        {
            this.satifiableCallBack = satifiableCallBack;
            this.unsatifiableCallBack = unsatifiableCallBack;
            this.timedoutCallBack = timedoutCallBack;
            this.errorCallBack = errorCallBack;

        }
        public void InitializeGenerator(int cpus, int timeout)
        {
            this.cpus = cpus;
            this.timeout = timeout;
        }

        virtual protected void initializeGenerator()
        {
            solver.maxDuration = timeout + 10;
        }

        virtual protected void startGenerator()
        {
            solver.Solve(aspCode, getAdditionalParameters(), false);
            waitingOnClingo = true;
        }

        virtual protected void finalizeGenerator()
        {
            waitingOnClingo = false;
        }

        virtual protected string getAdditionalParameters()
        {
            return $" --parallel-mode {cpus} --time-limit={timeout}";
        }

        virtual protected void SATISFIABLE(Clingo.AnswerSet answerSet, string jobID)
        {

            Debug.LogWarning("SATISFIABLE unimplemented");
        }

        virtual protected void UNSATISFIABLE(string jobID)
        {
            Debug.LogWarning("UNSATISFIABLE unimplemented");
        }

        virtual protected void TIMEDOUT(int time, string jobID)
        {
            Debug.LogWarning("TIMEDOUT unimplemented");
        }

        virtual protected void ERROR(string error, string jobID)
        {
            Debug.LogWarning("ERROR unimplemented");
        }
    }
}
