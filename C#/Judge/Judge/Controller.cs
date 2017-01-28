/*
 * Created by SharpDevelop.
 * User: JeremyGuo
 * Date: 2017/1/28
 * Time: 19:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;

namespace Judge
{
	/// <summary>
	/// Description of Controller.
	/// </summary>
	public class Controller
	{
		private bool loaded;
		private string current_path;
		private string contest_name;
		bool named;
		bool have_path;
		public class User{
			int user_id;
			string user_path;
			ArrayList scores;
			public User(){
				scores = new ArrayList();
				user_path = "";
				user_id = 0;
			}
		}
		public class TestPoint{
			int Time_lmt, Memory_lmt, points;
			string path, inname, outname;
			public TestPoint(){
				Time_lmt = Memory_lmt = -1;
				path = inname = outname = "";
			}
			public void Change(string pth, string ind, string outd, int tm, int mem, int pts){
				points = pts;
				Memory_lmt = mem;
				Time_lmt = tm;
				inname = ind;
				outname = outd;
				path = pth;
			}
		}
		public class Problem{
			ArrayList data;
			string data_path;
			string code_path;
			public Problem(){
				data_path = "./";
				code_path = "./";
				data = new ArrayList();
			}
			public void Add_Point(TestPoint tp){
				this.data.Add(tp);
			}
		}
		Problem tmp_problem;
		private ArrayList problems;
		private ArrayList users;
		int uid_counter;
		private void Clear(){
			uid_counter = 0;
			loaded = false;
			problems = new ArrayList();
			users = new ArrayList();
			named = false;
			have_path = false;
		}
		public Controller()
		{
			Clear();
		}
		public void Create_New_Contest(string c_name){
			Clear();
			loaded = true;
			contest_name = c_name;
			named = true;
		}
		public void Begin_Add_Problem(){
			tmp_problem = new Problem();
		}
		public void Add_Test_Point(string pth, string indata, string outdata, int time_lmt, int mem_lmt, int sc){
			TestPoint tmp = new TestPoint();
			tmp.Change(pth, indata, outdata, time_lmt, mem_lmt, sc);
			tmp_problem.Add_Point(tmp);
		}
		public void End_Add_Problem(){
			problems.Add(tmp_problem);
		}
	}
}
