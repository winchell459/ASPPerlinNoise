using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASP
{
    public class TrackGenerator : Generator
    {
        public int terrainWidth = 10, terrainHeight = 10;

        protected override string getASPCode()
        {
            string aspCode = @"

                #const terrainWidth = 10.
                #const terrainHeight = 10.

                width(1..terrainWidth).
                height(1..terrainHeight).

                terrainTypes(track;empty).

                1{tile(XX,YY,Type): terrainTypes(Type)}1 :- width(XX), height(YY).

                1{start(XX,YY): XX == 1, height(YY)}1.
                1{end(XX,YY): XX == terrainWidth, height(YY)}1.

                :- start(XX,YY), not tile(XX,YY,track).
                :- end(XX,YY), not tile(XX,YY,track).
                track(XX,YY, Count) :- tile(XX,YY,track),
                    Count = {tile(XX+1,YY,track);
                            tile(XX-1,YY,track);
                            tile(XX,YY+1,track);
                            tile(XX,YY-1,track)}.

                :- track(XX,YY,Count), start(XX,YY), Count != 1.
                :- track(XX,YY,Count), end(XX,YY), Count != 1.
                :- track(XX,YY,Count), not start(XX,YY), not end(XX,YY), Count != 2.

                :- start(XX,YY), end(XX,YY).

                path(XX,YY) :- start(XX,YY).
                path(XX,YY) :- track(XX,YY,_), path(XX-1,YY).
                path(XX,YY) :- track(XX,YY,_), path(XX+1,YY).
                path(XX,YY) :- track(XX,YY,_), path(XX,YY-1).
                path(XX,YY) :- track(XX,YY,_), path(XX,YY+1).

                :- track(XX,YY,_), not path(XX,YY).
                
                
                #show width/1.
                #show height/1.
                #show tile/3.

            ";
            return aspCode;
        }

        protected override string getAdditionalParameters()
        {
            return $"-c terrainWidth={terrainWidth} -c terrainHeight={terrainHeight}" + base.getAdditionalParameters();
        }
    }
}

