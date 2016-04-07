using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using xwcs.core.statemachine;

namespace WF2.States.document { // StateMachineNamespace
	public partial class ValidatoVersionatoState : StateBase 
	{ 
		public ValidatoVersionatoState(StateMachine machine) : base(machine, "ValidatoVersionato") { }

        /// <summary>
        /// Returns a list of callable triggers
        /// </summary>
        public override List<TriggerBase> GetTriggers()
        {
			List<TriggerBase> l = new List<TriggerBase>();
			l.Add(new RevertTrigger(StateMachine) ) ;
			l.Add(new PublishTrigger(StateMachine) ) ;
			l.Add(new SimpleFixTrigger(StateMachine) ) ;
			return l ;
        }

	}

}
// End of Tempate