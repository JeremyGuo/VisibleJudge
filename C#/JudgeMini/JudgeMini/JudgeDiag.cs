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
using System.Runtime.InteropServices;

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
		
		[DllImport("Judge_v1.0.2")]
		private static extern void Judge(string exe, string indata, string outdata, string userdata, string result, ref int ans, ref int tm, ref int mem);
		
		ArrayList data;
		Thread judging_thread;
		string in_pre, in_bak, out_pre, out_bak, execute;
		public void Init(ArrayList datas, string ip, string ib, string op, string ob, string ex){
			data = datas;
			in_pre = ip;
			in_bak = ib;
			out_pre = op;
			out_bak = ob;
			execute = ex;
		}
		void Init_Show(){
			int sz = data.Count;
			listView1.BeginUpdate();
			for(int i=0;i<sz;i++){
				ListViewItem lvi = new ListViewItem();
				lvi.SubItems.Add((string)data[i]);
				lvi.SubItems.Add("Pending");
				lvi.SubItems.Add("不可用");
				lvi.SubItems.Add("不可用");
				
				for(int j=0;j<4;j++)
					lvi.SubItems[j].BackColor = Color.LightGray;
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
				string in_name = in_pre + data[i] + in_bak;
				string out_name = out_pre + data[i] + out_bak;
				
				listView1.Items[i].SubItems[1].BackColor = Color.Yellow;
				listView1.Items[i].SubItems[1].Text = "Judging";
				
				int tm=0, mem=0, stat=0;
				Judge(execute+".exe", in_name, out_name, "tmp.out", "tmp.result.out", ref stat, ref tm, ref mem);
				
				if(stat == 0){
					listView1.Items[i].SubItems[1].BackColor = Color.Green;
					listView1.Items[i].SubItems[1].Text = "Accepted";
				}else if(stat == 1){
					listView1.Items[i].SubItems[1].BackColor = Color.Red;
					listView1.Items[i].SubItems[1].Text = "Wrong Answer";
				}else if(stat == 2){
					listView1.Items[i].SubItems[1].BackColor = Color.Red;
					listView1.Items[i].SubItems[1].Text = "Runtime Error";
				}else if(stat == 3){
					listView1.Items[i].SubItems[1].BackColor = Color.Yellow;
					listView1.Items[i].SubItems[1].Text = "Permutation Error";
				}
				
				listView1.Items[i].SubItems[2].Text = tm.ToString();
				listView1.Items[i].SubItems[3].Text = mem.ToString();
			}
			this.button1.Text = "关闭";
		}
		void Button1Click(object sender, EventArgs e)
		{
			judging_thread.Abort();
			this.Close();
			this.Dispose();
		}
	}
}
