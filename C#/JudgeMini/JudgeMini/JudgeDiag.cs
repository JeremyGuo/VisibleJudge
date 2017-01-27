/*
 * Created by SharpDevelop.
 * User: JeremyGuo
 * Date: 2017/1/27
 * Time: 15:20
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Collections;

namespace JudgeMini
{
	/// <summary>
	/// Description of JudgeDiag.
	/// </summary>
	public partial class JudgeDiag : Form
	{
		public JudgeDiag()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.ControlBox = false;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		ArrayList data;
		Thread judging_thread;
		public void Init(ArrayList datas){
			data = datas;
		}
		void Init_Show(){
			int sz = data.Count;
			listView1.BeginUpdate();
			for(int i=0;i<sz;i++){
				ListViewItem lvi = ListViewItem();
				lvi.SubItems.Add((string)data[i]);
				lvi.SubItems.Add("Pending");
				lvi.SubItems.Add("不可用");
				lvi.SubItems.Add("不可用");
				listView1.Items.Add(lvi);
			}
			listView1.EndUpdate();
		}
		void JudgeDiagShow(object sender, EventArgs e)
		{
			Init_Show();
			judging_thread = new Thread(Judge);
			judging_thread.Start();
		}
		void Judge(){
			int sz = data.Count;
			for(int i=0;i<sz;i++){
				
			}
		}
		void Button1Click(object sender, EventArgs e)
		{
			judging_thread.Abort();
			this.Close();
			this.Dispose();
		}
	}
}
