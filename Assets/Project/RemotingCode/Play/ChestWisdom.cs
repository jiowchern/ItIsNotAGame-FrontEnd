﻿using System;
using System.Linq;
using System.Linq.Expressions;

using Regulus.BehaviourTree;
using Regulus.Project.ItIsNotAGame1.Data;
using Regulus.Utility;

namespace Regulus.Project.ItIsNotAGame1.Game.Play
{
    internal class ChestWisdom : Wisdom 
    {

        

        private readonly IMapGate _Gate;

        private readonly Entity _Owner;
        

        private readonly IMapFinder _Finder;

        private readonly Entity _Exit;

        private readonly Entity _Door;

        private readonly Entity _Chest;

        

        
        
        

        public ChestWisdom(Entity owner, Entity exit, Entity debirs, Entity door, IMapFinder finder, IMapGate gate)
        {
            _Owner = owner;
            _Exit = exit;
            _Chest = debirs;
            _Door = door;
            _Finder = finder;
            _Gate = gate;
        }


        protected override void _Update(float delta)
        {
            
        }

        protected override void _Shutdown()
        {
        }

        protected override ITicker _Launch()
        {            
            var builder = new Regulus.BehaviourTree.Builder();
            var ticker = builder
                    .Sequence()
                        .Action(() => new ChestIdleAction(_Chest, _Gate))
                        .Action(() => new ChestExcludeAction(_Gate , _Finder, _Owner , _Door  , _Exit ))
                    .End()
                .Build();
            return ticker;
        }

        

    }
}
