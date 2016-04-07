using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using xwcs.core.statemachine;

namespace WF2.States.document { // StateMachineNamespace

	public partial class SetCopyNumberGuard {
        public override void Execute()
        {
            // Will throw a GuardException when condition goes wrong
			// Check SetCopyNumber
			var confirmResult =  MessageBox.Show("Please confirm that condition 'SetCopyNumber' is valid",
                                    "Confirm Status Change",
                                    MessageBoxButtons.YesNo);
			if (confirmResult != DialogResult.Yes) {
				throw new GuardException(-1, "Condition 'SetCopyNumber' not valid") ;
			}
            return;
        }
	}

}
// End of Tempate
