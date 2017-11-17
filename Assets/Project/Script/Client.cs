﻿using UnityEngine;
using System.Collections;
using System.Linq.Expressions;
using Regulus.Framework;
using Regulus.Project.ItIsNotAGame1;
using Regulus.Project.ItIsNotAGame1.Data;
using Regulus.Project.ItIsNotAGame1.Game.Play;
using Regulus.Utility;
using System;
using Regulus.Network.Rudp;
using Regulus.Remoting;


public class Client : MonoBehaviour , ICore
{
    
	public static Client Instance {
		get { return GameObject.FindObjectOfType<Client>();  }
	}
	public enum MODE
	{
		STANDALONE,REMOTING
	}

	
	public ResourceOwner Resource;
	public MODE Mode;
	public Console Console;

   
	private Client<IUser> _Client;
	private readonly Regulus.Utility.Updater _Updater;

	private IOnline _Online;

    private Center _Center;
    private Regulus.Network.Rudp.Client _Rudp;
    public Client()
	{
	    _Rudp = new Regulus.Network.Rudp.Client(new UdpSocket());
        var feature = new Regulus.Project.ItIsNotAGame1.Game.DummyFrature();
        _Center= new Center(feature, feature);
        _Updater = new Updater();
		
	}
	// Use this for initialization
	void Start ()
	{
		Mode = SceneChanger.Mode;
		Application.logMessageReceived += _Log;

#if !UNITY_STANDALONE_WIN
		Regulus.Utility.SpinWait.NotWindowsPlatform();
#endif
		var client = new Regulus.Framework.Client<Regulus.Project.ItIsNotAGame1.IUser>(Console, Console.Command );
		client.ModeSelectorEvent += _ToMode;

		
		_Client = client;
		_Updater.Add(_Client);
		Debug.Log("Started .");

	    _Rudp.Launch();

    }
	

	private void _Log(string condition, string stacktrace, LogType type)
	{
		Regulus.Utility.Log.Instance.WriteInfo(condition);
	}

	private void _SupplyOnline(IOnline obj)
	{
		_Online = obj;
	}

	private void _ToMode(GameModeSelector<IUser> selector)
	{
	    
        var gpiProvider = new Regulus.Project.ItIsNotAGame.Data.Protocol();
        UserProvider<IUser> provider;
		if (Mode == MODE.REMOTING)
		{
			selector.AddFactoty("r", new RemotingUserFactory(gpiProvider,_Rudp));
			provider = selector.CreateUserProvider("r");
		}
		else
		{
			this.Resource.Load();
			
			
			_Updater.Add(_Center);
			selector.AddFactoty("s", new StandaloneUserFactory(this, gpiProvider));
			provider = selector.CreateUserProvider("s");
		}

		var user = provider.Spawn("user");
		provider.Select("user");

		User = user;
		User.Remoting.OnlineProvider.Supply += _SupplyOnline;
		User.Remoting.OnlineProvider.Unsupply += _UnsupplyOnline;
		User.JumpMapProvider.Supply += _JumpMap;
	}

	private void _JumpMap(IJumpMap obj)
	{
		
		SceneChanger.Instance.ToRealm(obj.Realm);
		obj.Ready();
	}

	public IUser User { get; private set; }

	public float Ping
	{
		get
		{
			if (_Online != null)
				return (float)_Online.Ping;
			return 0.0f;
		}
	}

	// Update is called once per frame
    void ICore.Launch(IProtocol protocol, ICommand command)
    {
        
    }

    bool ICore.Update()
    {
        return true;
    }

    void ICore.Shutdown()
    {
        
    }

    void Update () {

	    try
	    {
	        _Updater.Working();
	    }
	    catch (Regulus.DeserializeException de)
	    {
            Debug.Log(de);
        }
		/*catch(Exception e)
		{
			Debug.Log(e);
			
		}*/
		
	}

	void OnDestroy()
	{
		User.JumpMapProvider.Supply -= _JumpMap;
		User.Remoting.OnlineProvider.Supply -= _SupplyOnline;
		User.Remoting.OnlineProvider.Unsupply -= _UnsupplyOnline;
		_Updater.Shutdown();
	    _Rudp.Shutdown();

    }

	private void _UnsupplyOnline(IOnline obj)
	{
		_Online = null;
	}

    void IBinderProvider.AssignBinder(ISoulBinder binder)
    {
        _Center.AssignBinder(binder);
    }
}
