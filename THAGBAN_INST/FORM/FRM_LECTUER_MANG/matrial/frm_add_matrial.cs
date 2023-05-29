﻿using DevExpress.XtraEditors;
using DevExpress.XtraScheduler.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using THAGBAN_INST.DATA;

namespace THAGBAN_INST.FORM.FRM_LECTUER_MANG.matrial
{
    public partial class frm_add_matrial : DevExpress.XtraEditors.XtraForm
    {

        db_max_instEntities con = new db_max_instEntities();
        tost toast = new tost();
        dialge dialge = new dialge();
        public int matrial_id=0;

        public string matrial_name;
        public int cours_id;
        public int lect_id;


        public frm_add_matrial()
        {
            InitializeComponent();
            get_matriaL();
        }

        
        private void btn_delete_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frm_add_job_Load(object sender, EventArgs e)
        {
           
            if (matrial_id != 0)
            {
               TBL_LECT_MATRIAL tbl= con.TBL_LECT_MATRIAL.Find(matrial_id);
                txt_matrial_name.Text = tbl.MATRIAL_LECT_NAME.ToString();
                cours_id =Convert.ToInt32(tbl.LECT_COURS_ID);
                com_com_cours.DataSource = con.TBL_LECT_COURS.ToList();
                com_com_cours.DisplayMember = "LECT_COURS_NAME";
                com_com_cours.ValueMember = "LECT_COURS_ID";
                com_com_cours.SelectedValue = cours_id;
                lect_id = Convert.ToInt32(tbl.LECT_ID);
                //   com_term_year.Text = con.TBL_TERMS.Find(tech_id).SPEC_YEAR.ToString();
                com_lect.DataSource = con.TBL_LECTUER.ToList();
                com_lect.DisplayMember = "LECT_NAME";
                com_lect.ValueMember = "LECT_ID";
                com_lect.SelectedValue = lect_id;
            }
            else
            {
                get_data();
            }
           
        }
     
        public void get_data()
        {

            // This line of code is generated by Data Source Configuration Wizard
            // Instantiate a new DBContext
            db_max_instEntities dbContext = new db_max_instEntities();
            // Call the LoadAsync method to asynchronously get the data for the given DbSet from the database.
            dbContext.TBL_LECT_COURS.LoadAsync().ContinueWith(loadTask =>
            {
                // Bind data to control when loading complete
                com_com_cours.DataSource = dbContext.TBL_LECT_COURS.Local.ToBindingList();

               
                com_com_cours.DisplayMember = "LECT_COURS_NAME";
                com_com_cours.ValueMember = "LECT_COURS_ID";
                //   com_term_year.Text = con.TBL_TERMS.Find(tech_id).SPEC_YEAR.ToString();
                
               
            }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
        }
        void clear()
        {
            lect_id =0;
            matrial_id = 0;
            com_lect.Text = "";
            txt_matrial_name.Text = "";
            com_com_cours.Text = "";


        }
        bool is_empty()
        {
            if (com_lect.Text != "" && txt_matrial_name.Text != "" && com_com_cours.Text != "")
            {
                return false;
            }
            else
                return true;

        }
        void get_matriaL()
        {
            try
            {
                AutoCompleteStringCollection source = new AutoCompleteStringCollection();
                var temp = con.TBL_LECT_MATRIAL.ToList();
                if (temp.Count > 0)
                {
                    foreach (var item in temp)
                    {
                        source.Add(item.MATRIAL_LECT_NAME);
                    }
                }
                txt_matrial_name.AutoCompleteCustomSource = source;
            }catch(Exception ex)
            {

            }
        }

        void add_terms()
        {

            tost toast = new tost();
            dialge dialge = new dialge();
            if (is_empty() != true)
            {
                //cheak add or edit 
                try
                {
                    TBL_LECT_MATRIAL cl = new TBL_LECT_MATRIAL();
                    cl.MATRIAL_LECT_NAME = txt_matrial_name.Text;
                    cl.LECT_ID =Convert.ToInt32(com_lect.SelectedValue);
                    cl.LECT_COURS_ID = Convert.ToInt32(com_com_cours.SelectedValue);

                    if (matrial_id != 0)
                    {
                        //add 
                        cl.MATRIAL_LECT_ID = Convert.ToInt32(matrial_id);
                        con.TBL_LECT_MATRIAL.AddOrUpdate(cl);
                        con.SaveChanges();
                        toast.Width = this.Width;

                        adl.NotifictionUser notifiction = new adl.NotifictionUser(THAGBAN_INST.Properties.Resources.EditNotificationText, THAGBAN_INST.Properties.Resources.edit_32px);
                        notifiction.Show();
                        clear();
                        //MessageBox.Show("تم التعديل بنجاح ");
                    }
                    else
                    {
                        //update 
                        con.TBL_LECT_MATRIAL.AddOrUpdate(cl);
                        con.SaveChanges();
                        toast.Width = this.Width;
                        adl.NotifictionUser notifiction = new adl.NotifictionUser(THAGBAN_INST.Properties.Resources.AddNotificationText, THAGBAN_INST.Properties.Resources.add_32px);
                        notifiction.Show();
                        clear();


                    }
                }
                catch (Exception ex)
                {

                    //dialge.Width = this.Width;
                    dialge.lbl_mess.Text ="لايمكن اضافه الترم " +
                        "لنفس السنه لنفس التخصص ";
                    dialge.Show();
                }
                clear();
            }
            else
            {
                dialge.Width = this.Width;
                dialge.lbl_mess.Text = "الرجاء ادخال جميع البيانات ";
                dialge.Show();



            }


        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            add_terms();
        }

        private void com_spiacl_SelectionChangeCommitted(object sender, EventArgs e)
        {
          
            if (com_com_cours.SelectedValue!= null)
            {
                cours_id=Convert.ToInt32(com_com_cours.SelectedValue.ToString());
               com_lect.DataSource=con.TBL_LECTUER.Where(w=>w.LECT_COURS_ID==cours_id).ToList();
                com_lect.DisplayMember = "LECT_NAME";
                com_lect.ValueMember = "LECT_ID";


            }
        }

        private void com_spiacl_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void com_term_year_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }
    }
}