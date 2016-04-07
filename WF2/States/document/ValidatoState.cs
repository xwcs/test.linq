using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using xwcs.core.statemachine;

namespace WF2.States.document { // StateMachineNamespace
	public partial class ValidatoState : StateBase 
	{ 
		public ValidatoState(StateMachine machine) : base(machine, "Validato") { }

        /// <summary>
        /// Returns a list of callable triggers
        /// </summary>
        public override List<TriggerBase> GetTriggers()
        {
			List<TriggerBase> l = new List<TriggerBase>();
			l.Add(new InvalidateTrigger(StateMachine) ) ;
			l.Add(new PublishTrigger(StateMachine) ) ;
			l.Add(new VersionTrigger(StateMachine) ) ;
			l.Add(new SimpleFixTrigger(StateMachine) ) ;
			l.Add(new RejectTrigger(StateMachine) ) ;
			return l ;
        }

	}

}
// End of Tempate