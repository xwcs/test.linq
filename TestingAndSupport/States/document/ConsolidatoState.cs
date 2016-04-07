using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using xwcs.core.statemachine;

namespace WF2.States.document { // StateMachineNamespace
	public partial class ConsolidatoState : StateBase 
	{ 
		public ConsolidatoState(StateMachine machine) : base(machine, "Consolidato") { }

        /// <summary>
        /// Returns a list of callable triggers
        /// </summary>
        public override List<TriggerBase> GetTriggers()
        {
			List<TriggerBase> l = new List<TriggerBase>();
			l.Add(new RejectTrigger(StateMachine) ) ;
			l.Add(new ValidateTrigger(StateMachine) ) ;
			return l ;
        }

	}

}
// End of Tempate