using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using xwcs.core.statemachine;

namespace WF2.States.document { // StateMachineNamespace

	public partial class NotFallGuard {
        public override void Execute()
        {
            // Will throw a GuardException when condition goes wrong
			// Check NotFall
			var confirmResult =  MessageBox.Show("Please confirm that condition 'NotFall' is valid",
                                    "Confirm Status Change",
                                    MessageBoxButtons.YesNo);
			if (confirmResult != DialogResult.Yes) {
				throw new GuardException(-1, "Condition 'NotFall' not valid") ;
			}
            return;
        }
	}

}
// End of Tempate
