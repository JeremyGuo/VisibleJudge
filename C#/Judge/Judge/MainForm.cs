﻿/*
 * Created by SharpDevelop.
 * User: JeremyGuo
 * Date: 2017/1/28
 * Time: 19:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Judge
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
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
		void 添加题目ToolStripMenuItemClick(object sender, EventArgs e)
		{
	
		}
		void 评测部分题目ToolStripMenuItemClick(object sender, EventArgs e)
		{
	
		}
		void Form2_FormClosing(object sender, FormClosingEventArgs e)
		{
			DialogResult result = MessageBox.Show("你确定要关闭吗！", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
			if(result == DialogResult.OK){
				e.Cancel = false;
			}else e.Cancel = true;
		}
	}
}
