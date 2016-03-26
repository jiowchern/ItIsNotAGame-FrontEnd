﻿using System;

using Regulus.CustomType;
using Regulus.Project.ItIsNotAGame1.Data;
using Regulus.Utility;

namespace Regulus.Project.ItIsNotAGame1.Game.Play
{
    public class EntityProvider 
    {
        public static Entity Create(GamePlayerRecord record)
        {
            var data = Singleton<Resource>.Instance.FindEntity(record.Entity);
            return new Entity(data, record.Name);
        }

        public static Entity Create(ENTITY type)
        {
            var data = Singleton<Resource>.Instance.FindEntity(type);
            return new Entity(data);
        }

        public static Entity CreateResource(ENTITY type , Inventory inventory)
        {
            var data = Singleton<Resource>.Instance.FindEntity(type);
            var entity = new Entity(data , inventory);
            return entity;
        }

        public static Entity CreateEnterance(ENTITY[] types)
        {
            var data = Singleton<Resource>.Instance.FindEntity(ENTITY.ENTRANCE);            
            return new Entity(data , "", types);
        }
    }
}