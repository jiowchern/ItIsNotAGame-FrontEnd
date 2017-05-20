                    

namespace Regulus.Project.ItIsNotAGame1.Data.Adsorption
{
    using System.Linq;
        
    public class AccountStatusAdsorber : UnityEngine.MonoBehaviour , Regulus.Remoting.Unity.Adsorber<IAccountStatus>
    {
        private readonly Regulus.Utility.StageMachine _Machine;        
        
        public string Agent;

        private global::Regulus.Project.ItIsNotAGame.Data.Agent _Agent;

        [System.Serializable]
        public class UnityEnableEvent : UnityEngine.Events.UnityEvent<bool> {}
        public UnityEnableEvent EnableEvent;
        [System.Serializable]
        public class UnitySupplyEvent : UnityEngine.Events.UnityEvent<Regulus.Project.ItIsNotAGame1.Data.IAccountStatus> {}
        public UnitySupplyEvent SupplyEvent;
        Regulus.Project.ItIsNotAGame1.Data.IAccountStatus _AccountStatus;                        
       
        public AccountStatusAdsorber()
        {
            _Machine = new Regulus.Utility.StageMachine();
        }

        void Start()
        {
            _Machine.Push(new Regulus.Utility.SimpleStage(_ScanEnter, _ScanLeave, _ScanUpdate));
        }

        private void _ScanUpdate()
        {
            var agents = UnityEngine.GameObject.FindObjectsOfType<global::Regulus.Project.ItIsNotAGame.Data.Agent>();
            _Agent = agents.FirstOrDefault(d => string.IsNullOrEmpty(d.Name) == false && d.Name == Agent);
            if(_Agent != null)
            {
                _Machine.Push(new Regulus.Utility.SimpleStage(_DispatchEnter, _DispatchLeave));
            }            
        }

        private void _DispatchEnter()
        {
            _Agent.Distributor.Attach<IAccountStatus>(this);
        }

        private void _DispatchLeave()
        {
            _Agent.Distributor.Detach<IAccountStatus>(this);
        }

        private void _ScanLeave()
        {

        }


        private void _ScanEnter()
        {

        }

        void Update()
        {
            _Machine.Update();
        }

        void OnDestroy()
        {
            _Machine.Termination();
        }

        public Regulus.Project.ItIsNotAGame1.Data.IAccountStatus GetGPI()
        {
            return _AccountStatus;
        }
        public void Supply(Regulus.Project.ItIsNotAGame1.Data.IAccountStatus gpi)
        {
            _AccountStatus = gpi;
            _AccountStatus.KickEvent += _OnKickEvent;
            EnableEvent.Invoke(true);
            SupplyEvent.Invoke(gpi);
        }

        public void Unsupply(Regulus.Project.ItIsNotAGame1.Data.IAccountStatus gpi)
        {
            EnableEvent.Invoke(false);
            _AccountStatus.KickEvent -= _OnKickEvent;
            _AccountStatus = null;
        }
        
        
        
        [System.Serializable]
        public class UnityKickEvent : UnityEngine.Events.UnityEvent { }
        public UnityKickEvent KickEvent;
        
        
        private void _OnKickEvent()
        {
            KickEvent.Invoke();
        }
        
    }
}
                    