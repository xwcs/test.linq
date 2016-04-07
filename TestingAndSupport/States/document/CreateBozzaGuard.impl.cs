using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using xwcs.core.statemachine;

namespace WF2.States.document { // StateMachineNamespace

	public partial class CreateBozzaGuard {
        public override void Execute()
        {
            // Will throw a GuardException when condition goes wrong
			// Check CreateBozza
			var confirmResult =  MessageBox.Show("Please confirm that condition 'CreateBozza' is valid",
                                    "Confirm Status Change",
                                    MessageBoxButtons.YesNo);
			if (confirmResult != DialogResult.Yes) {
				throw new GuardException(-1, "Condition 'CreateBozza' not valid") ;
			}
            return;
        }
	}

}
// End of Tempate
