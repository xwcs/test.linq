using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using xwcs.core.statemachine;

namespace WF2.States.document { // StateMachineNamespace
	public partial class PubblicatoState : StateBase 
	{ 
		public PubblicatoState(StateMachine machine) : base(machine, "Pubblicato") { }

        /// <summary>
        /// Returns a list of callable triggers
        /// </summary>
        public override List<TriggerBase> GetTriggers()
        {
			List<TriggerBase> l = new List<TriggerBase>();
			l.Add(new VersionTrigger(StateMachine) ) ;
			l.Add(new HideTrigger(StateMachine) ) ;
			l.Add(new SimpleFixTrigger(StateMachine) ) ;
			return l ;
        }

	}

}
// End of Tempate