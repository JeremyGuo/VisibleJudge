/*
 * Created by SharpDevelop.
 * User: JeremyGuo
 * Date: 2017/1/27
 * Time: 11:28
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Threading;

namespace JudgeMini
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private JudgeDiag diag;
		private ArrayList arr;
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		bool isEmpty(string str){
			return (str.Trim() == "");
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			string execute_name, in_pre, in_bak, out_pre, out_bak;
			string in_full_name, out_full_name;
			string extra_args;
			
			execute_name = textBox1.Text;
			in_full_name = textBox3.Text;
			out_full_name = textBox4.Text;
			extra_args = textBox2.Text;
			
			if(isEmpty(execute_name) || isEmpty(in_full_name) || isEmpty(out_full_name)){
				MessageBox.Show("存在必须参数为空", "信息错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return ;
			}
			
			if(in_full_name.Split('#').Length != 2 || out_full_name.Split('#').Length != 2){
				MessageBox.Show("输入输出文件格式错误", "信息错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return ;
			}
			
			in_pre = in_full_name.Split('#')[0];
			in_bak = in_full_name.Split('#')[1];
			
			out_pre = out_full_name.Split('#')[0];
			out_bak = out_full_name.Split('#')[1];
			
			DirectoryInfo dir = new DirectoryInfo(".");
			FileInfo[] in_files = dir.GetFiles(in_pre + "*" + in_bak);
			arr = new ArrayList();
			
			int sz = in_files.Length;
			for(int i=0;i<sz;i++){
				string tmp_name = in_files[i].Name;
				string id = tmp_name.Split(new string[]{in_pre}, StringSplitOptions.RemoveEmptyEntries)[0];
				id = id.Split(new String[]{in_bak}, StringSplitOptions.RemoveEmptyEntries)[0];
				
				if(dir.GetFiles(out_pre + id + out_bak).Length != 0)
					arr.Add(id);
			}
			diag = new JudgeDiag();
			diag.Init(arr);
			diag.ShowDialog();
		}
	}
}
