/*
 * Greenshot - a free and open source screenshot tool
 * Copyright (C) 2007-2010  Thomas Braun, Jens Klingen, Robin Krom
 * 
 * For more information see: http://getgreenshot.org/
 * The Greenshot project is hosted on Sourceforge: http://sourceforge.net/projects/greenshot/
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 1 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Drawing;
using System.Net;
using System.Web;
using System.Windows.Forms;

using Greenshot.Configuration;
using Greenshot.Helpers;
using GreenshotPlugin.Core;

namespace Greenshot.Forms {
	public partial class BugReportForm : Form {
		private ILanguage lang;
		private BugReportForm() {
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			lang = Language.GetInstance();
			UpdateUI();
		}
		
		
		public BugReportForm(string bugText) {
			InitializeComponent();
			lang = Language.GetInstance();
			UpdateUI();
			this.textBoxDescription.Text = bugText;
		}
		
		void UpdateUI() {
			this.Text = lang.GetString(LangKey.bugreport_title);
			this.labelBugReportInfo.Text = lang.GetString(LangKey.bugreport_info);
			this.btnClose.Text = lang.GetString(LangKey.bugreport_cancel);
		}
		
		void LinkLblBugsLinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e) {
			openLink((LinkLabel)sender);
		}

		private void openLink(LinkLabel link) {
			try {
				link.LinkVisited = true;
   				System.Diagnostics.Process.Start(link.Text);
			} catch (Exception) {
				MessageBox.Show(lang.GetString(LangKey.error_openlink),lang.GetString(LangKey.error));
			}
		}
	}
}