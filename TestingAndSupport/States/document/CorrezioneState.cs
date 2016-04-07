using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using xwcs.core.statemachine;

namespace WF2.States.document { // StateMachineNamespace
	public partial class CorrezioneState : StateBase 
	{ 
		public CorrezioneState(StateMachine machine) : base(machine, "Correzione") { }

        /// <summary>
        /// Returns a list of callable triggers
        /// </summary>
        public override List<TriggerBase> GetTriggers()
        {
			List<TriggerBase> l = new List<TriggerBase>();
			l.Add(new SaveTrigger(StateMachine) ) ;
			return l ;
        }

	}

}
// End of Tempate