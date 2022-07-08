using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASP
{
    public class TrackGenerator_2 : Generator
    {
        public int terrainWidth = 20, terrainHeight = 20;
        public MemoryScriptableObject trackMemory;
        protected override string getAdditionalParameters()
        {
            return $" -c terrainWidth={terrainWidth} -c terrainHeight={terrainHeight} " + base.getAdditionalParameters();
        }

        protected override string getASPCode()
        {
            string aspCode = @"

                #const terrainWidth = 20.
                #const terrainHeight = 20.

                terrainTypes(track; empty).
                width(1..terrainWidth).
                height(1..terrainHeight).

                %1{tile(XX,YY,Type): terrainTypes(Type)}1 :- width(XX), height(YY).

                %1{start(XX,YY): XX == 1, height(YY)}1.
                %1{end(terrainWidth, YY): height(YY)}1.
                1{start(XX,YY): width(XX), height(YY)}1.
                1{end(XX +2, YY): start(XX,YY)}1.
                %:- Count = {track(_,_,_)}, Count < 4.

                :- start(XX,YY), not tile(XX,YY,track).
                :- end(XX,YY), not tile(XX,YY,track).


                %% -- tileRules -- %%
                %track(XX,YY,Count) :- tile(XX,YY,track), Count = {tile(XX+1,YY,track); tile(XX-1,YY,track); tile(XX,YY+1,track); tile(XX,YY-1,track)}.
                %:- track(XX,YY,Count), not start(XX,YY), not end(XX,YY), Count != 2.
                %:- track(XX,YY,Count), start(XX,YY), Count != 1.
                %:- track(XX,YY,Count), end(XX,YY), Count != 1.

                


                %% -- pathRules -- %%
                path(XX,YY) :- start(XX,YY).
                
                {path(XX,YY)} :- tile(XX,YY,track), path(XX+1,YY).
                {path(XX,YY)} :- tile(XX,YY,track), path(XX-1,YY).
                {path(XX,YY)} :- tile(XX,YY,track), path(XX,YY+1).
                {path(XX,YY)} :- tile(XX,YY,track), path(XX,YY-1).
                %:- tile(XX,YY,track), not path(XX,YY).
                :- end(XX,YY), not path(XX,YY).

                track(XX,YY,Count) :- path(XX,YY), Count = {path(XX+1,YY); path(XX-1,YY); path(XX,YY+1); path(XX,YY-1)}.

                tileTrack(XX,YY,track) :- track(XX,YY, _).
                tileTrack(XX,YY,empty) :- tile(XX,YY,track), not track(XX,YY,_).
                :- Count = {path(_,_)}, Count < 4.
                :- track(XX,YY,Count), Count != 2.
                
                #show tileTrack/3.
                #show width/1.
                #show height/1.

            ";
            return aspCode + trackMemory.GetASPCode();
        }
    }

}

