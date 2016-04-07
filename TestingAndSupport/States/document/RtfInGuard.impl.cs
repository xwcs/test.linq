using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using xwcs.core.statemachine;

namespace WF2.States.document { // StateMachineNamespace

	public partial class RtfInGuard {
        public override void Execute()
        {
            // Will throw a GuardException when condition goes wrong
			// Check RtfIn
			var confirmResult =  MessageBox.Show("Please confirm that condition 'RtfIn' is valid",
                                    "Confirm Status Change",
                                    MessageBoxButtons.YesNo);
			if (confirmResult != DialogResult.Yes) {
				throw new GuardException(-1, "Condition 'RtfIn' not valid") ;
			}
            return;
        }
	}

}
// End of Tempate
