
    using System;  
    using System.Collections.Generic;
    
    namespace Regulus.Project.ItIsNotAGame1.Data.Event.IVisible 
    { 
        public class EnergyEvent : Regulus.Remoting.IEventProxyCreator
        {

            Type _Type;
            string _Name;
            
            public EnergyEvent()
            {
                _Name = "EnergyEvent";
                _Type = typeof(Regulus.Project.ItIsNotAGame1.Data.IVisible);                   
            
            }
            Delegate Regulus.Remoting.IEventProxyCreator.Create(Guid soul_id, Action<Guid, string, object[]> invoke_Event)
            {                
                var closure = new Regulus.Remoting.GenericEventClosure<Regulus.Project.ItIsNotAGame1.Data.Energy>(soul_id , _Name , invoke_Event);                
                return new Action<Regulus.Project.ItIsNotAGame1.Data.Energy>(closure.Run);
            }
        

            Type Regulus.Remoting.IEventProxyCreator.GetType()
            {
                return _Type;
            }            

            string Regulus.Remoting.IEventProxyCreator.GetName()
            {
                return _Name;
            }            
        }
    }
                