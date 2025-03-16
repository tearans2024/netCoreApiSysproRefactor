using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using System.Data;
using WebApplication5;

namespace netCoreApiSyspro
{
    public class FunctionClass
    {

        public string SetSetring(object Str)
        {
            String SetSetring = "Null";

            if (Str == System.DBNull.Value)
                return SetSetring;

            if (Strings.Len(Str) > 0)
                SetSetring = "'" + Strings.Replace(Str.ToString(), "'", "''") + "'";

            return SetSetring;
        }


        public string SetInteger(object Str)
        {
            String Hasil = "Null";

            if (Str == System.DBNull.Value)
                return Hasil;

            if (Strings.Len(Str) > 0)
                Hasil = "" + Str.ToString() + "";

            return Hasil;
        }
        public string SetDec(object Str)
        {
            String Hasil = "Null";

            if (Str == System.DBNull.Value)
                return Hasil;

            if (Strings.Len(Str) > 0)
                Hasil = "" + System.Convert.ToDouble(Str).ToString().Replace(",", ".") + "";

            return Hasil;
        }

        public string SetDec0(object Str)
        {
            String Hasil = "0";

            if (Str == System.DBNull.Value)
                return Hasil;

            if (Strings.Len(Str) > 0)
                Hasil = "" + System.Convert.ToDouble(Str).ToString().Replace(",", ".") + "";

            return Hasil;
        }

        public string SetDateNTime00(object Str)
        {
            String SetDateNTime00 = "Null";

            if (Str == System.DBNull.Value)
                return SetDateNTime00;

            if (Strings.Len(Str) > 0)
            {

                SetDateNTime00 = "'" + System.Convert.ToDateTime(Str).ToString("dd/MM/yyyy") + " 00:00:00" + "'";// Strings.Format((DateTime)Str, "dd/MM/yyyy")
            }
            return SetDateNTime00;
        }

        public string SetDateNTime99(object Str)
        {
            String SetDateNTime00 = "Null";

            if (Str == System.DBNull.Value)
                return SetDateNTime00;

            if (Strings.Len(Str) > 0)
            {

                SetDateNTime00 = "'" + System.Convert.ToDateTime(Str).ToString("dd/MM/yyyy") + " 23:59:59" + "'";//Strings.Format((DateTime)Str, "dd/MM/yyyy")
            }
            return SetDateNTime00;
        }
        public string SetDateNTime(object Str)
        {
            String SetDateNTime00 = "Null";

            if (Str == System.DBNull.Value)
                return SetDateNTime00;

            if (Strings.Len(Str) > 0)
            {

                SetDateNTime00 = "'" + System.Convert.ToDateTime(Str).ToString("dd/MM/yyyy HH:mm:ss") + "'";//Strings.Format((DateTime)Str, "dd/MM/yyyy HH:mm:ss")
            }
            return SetDateNTime00;
        }

        public double SetNumberDouble(object Str)
        {
            double hasil = 0;

            if (Str == System.DBNull.Value)
                return hasil;

            if (Strings.Len(Str) > 0)
                hasil = System.Convert.ToDouble(Str);

            return hasil;
        }
        public int SetNumberInteger(object Str)
        {
            int hasil = 0;

            if (Str == System.DBNull.Value)
                return hasil;

            if (Strings.Len(Str) > 0)
                hasil = System.Convert.ToInt32(Str);

            return hasil;
        }

        public string SetString(object Str)
        {
            String SetSetring = "";

            if (Str == System.DBNull.Value)
                return SetSetring;

            if (Strings.Len(Str) > 0)
                SetSetring = Str.ToString();

            return SetSetring;
        }


        public ArrayList FinsertSQL2Array(ArrayList List)
        {
            ArrayList sSqls = new ArrayList();
            string sSql;
            int i = 1;
            try
            {
                foreach (string Sql in List)
                {
                    sSql = "INSERT INTO  "
                        + "  public.t_sql_out "
                        + "( "
                        + "  sql_uid, "
                        + "  seq, "
                        + "  sql_command, "
                        + "  mili_second, "
                        + "  waktu "
                        + ")  "
                        + "VALUES ( "
                        + SetSetring(Guid.NewGuid().ToString()) + ",  "
                        + i + ",  "
                        + SetSetring(Sql) + ",  "
                        + "cast(to_char(now(),'MS') as integer)" + ",  "
                        + "now()" + "  "
                        + ")";

                    sSqls.Add(sSql);
                    sSql = "";

                    i = i + 1;
                }
                return sSqls;
            }
            catch (Exception ex)
            {
                //Pesan(Information.Err);
                return sSqls;
            }
        }



        public Object CekTanggal()
        {
            try
            {
                string sSQL;
                DataRow NewRow;


                sSQL = "SELECT LOCALTIMESTAMP as Tgl";

                NewRow = GetRowInfo(sSQL);

                return (DateTime)NewRow["Tgl"];
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }

        public String server_code()
        {
            try
            {
                String hasil = "";
                DataTable dt = new DataTable();
                dt = GetTableData("select coalesce(server_code,'zz') as server_code from tconfsetting");

                foreach (DataRow dr in dt.Rows)
                {
                    hasil = (string)dr[0];
                }
                return hasil;
            }
            catch (Exception e)
            {
                return "";
                throw e;
            }

        }
        public int userID()
        {
            return 777;
        }

        public long GetID(string par_table, string par_en_code, string par_colom, string par_colom_criteria, string criteria)
        {
            long hasil = 0;
            long id_lama = 0;
            try
            {


                DataTable dt = new DataTable();
                dt = GetTableData("select coalesce(max(cast(substring(cast(" + par_colom
                    + " as varchar),3,100) as integer)),0) as max_col  from " + par_table
                    + " where " + par_colom_criteria + " = '" + criteria + "'"
                    + " and substring(cast(" + par_colom + " as varchar),3,100) <> ''");

                foreach (DataRow dr in dt.Rows)
                {
                    id_lama = (int)dr[0] + 1;
                }


                //return hasil;

            }
            catch (Exception ex)
            {

            }

            if (par_en_code == "0")
                par_en_code = "99";

            hasil = System.Convert.ToInt64(par_en_code + id_lama.ToString());

            return hasil;
        }


        public string get_transaction_number(string par_type, string par_entity, string par_table, string par_colom)
        {

            String get_transaction_number = "";

            string tahun, bulan, no_urut_format;
            Int32 _no_urut;

            DateTime tanggal;
            tanggal = (DateTime)CekTanggal();

            tahun = tanggal.Year.ToString().Substring(2, 2);
            bulan = tanggal.Month.ToString();
            no_urut_format = "";
            try
            {
                if (Strings.Len(bulan) == 1)
                    bulan = "0" + bulan;


                //String sSQL = "select coalesce(max(cast(substring(" + par_colom + ",14,5) as integer)),0) + 1 as no_urut " + " from "
                //                + par_table + " where substring(" + par_colom + ",3,2) = '" + par_entity + "'" + " and substring(" + par_colom + ",5,2) = '"
                //                + tahun + "'" + " and substring(" + par_colom + ",7,2) = '" + bulan + "'" + " and length(" + par_colom + ")=18 limit 1";

                String sSQL = "select coalesce(max(cast(substring(" + par_colom + ",11,5) as integer)),0) + 1 as no_urut " + " from "
                               + par_table + " where substring(" + par_colom + ",3,2) = '" + par_entity + "'" + " and substring(" + par_colom + ",5,2) = '"
                               + tahun + "'" + " and substring(" + par_colom + ",7,2) = '" + bulan + "'" + " and length(" + par_colom + ")=18 limit 1";


                DataTable dt_bantu = new DataTable();
                dt_bantu = GetTableData(sSQL);

                _no_urut = 0;
                foreach (DataRow dr in dt_bantu.Rows)
                {
                    _no_urut = ((Int32)dr["no_urut"]);
                }

                //String _id_cus = "";
                //if (customer_id.Length >= 3){
                //    _id_cus = customer_id.Substring(customer_id.Length - 3);
                //} else
                //{
                //    _id_cus = customer_id;
                //}

                //int _id_cus_int = int.Parse(_id_cus);
                Random rnd = new Random();
                int num = rnd.Next(1, 999);

                string _no_id = num.ToString("000");

                no_urut_format = String.Format("{0:00000}", _no_urut);

                //get_transaction_number = par_type + par_entity + tahun + bulan + server_code() + _no_id + no_urut_format;
                get_transaction_number = par_type + par_entity + tahun + bulan + server_code() + no_urut_format + _no_id;

                return get_transaction_number;
            }
            catch (Exception ex)
            {

                return get_transaction_number;
                throw ex;
            }


        }

        public double get_ppn(int par_tax_class)
        {
            double get_ppn = 0;
            try
            {

                String sSQL = "select code_name, taxr_rate  " +
                                               " from taxr_mstr  " +
                                               " inner join code_mstr on code_id = taxr_tax_type " +
                                               " where taxr_tax_class = " + par_tax_class.ToString() +
                                               " and code_name ~~* 'PPN'";
                DataTable dt_bantu = new DataTable();
                dt_bantu = GetTableData(sSQL);

                get_ppn = 0;
                foreach (DataRow dr in dt_bantu.Rows)
                {
                    get_ppn = (double)dr["taxr_rate"] / 100;
                }
                return get_ppn;
            }
            catch (Exception ex)
            {
                return get_ppn;
                throw ex;
            }


        }

        public double get_pph(int par_tax_class)
        {
            double get_pph = 0;
            try
            {

                String sSQL = "select code_name, taxr_rate  " +
                                               " from taxr_mstr  " +
                                               " inner join code_mstr on code_id = taxr_tax_type " +
                                               " where taxr_tax_class = " + par_tax_class.ToString() +
                                               " and code_name ~~* 'PPH'";
                DataTable dt_bantu = new DataTable();
                dt_bantu = GetTableData(sSQL);

                get_pph = 0;
                foreach (DataRow dr in dt_bantu.Rows)
                {
                    get_pph = (double)dr["taxr_rate"] / 100;
                }
                return get_pph;
            }
            catch (Exception ex)
            {
                return get_pph;
                throw ex;
            }


        }

        public string Terbilang(long a)
        {
            try

            {

                string[] bilangan = new string[] { "", "Satu", "Dua", "Tiga", "Empat", "Lima", "Enam", "Tujuh", "Delapan", "Sembilan", "Sepuluh", "Sebelas" };
                var kalimat = "";
                // 1 - 11
                if (a < 12)
                {
                    kalimat = bilangan[a];
                }
                // 12 - 19
                else if (a < 20)
                {
                    kalimat = bilangan[a - 10] + " Belas";
                }
                // 20 - 99
                else if (a < 100)
                {
                    var utama = a / 10;
                    var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                    var belakang = a % 10;
                    kalimat = bilangan[depan] + " Puluh " + bilangan[belakang];
                }
                // 100 - 199
                else if (a < 200)
                {
                    kalimat = "Seratus " + Terbilang(a - 100);
                }
                // 200 - 999
                else if (a < 1000)
                {
                    var utama = a / 100;
                    var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                    var belakang = a % 100;
                    kalimat = bilangan[depan] + " Ratus " + Terbilang(belakang);
                }
                // 1,000 - 1,999
                else if (a < 2000)
                {
                    kalimat = "Seribu " + Terbilang(a - 1000);
                }
                // 2,000 - 9,999
                else if (a < 10000)
                {
                    var utama = a / 1000;
                    var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                    var belakang = a % 1000;
                    kalimat = bilangan[depan] + " Ribu " + Terbilang(belakang);
                }
                // 10,000 - 99,999
                else if (a < 100000)
                {
                    var utama = a / 100;
                    var depan = Convert.ToInt32(utama.ToString().Substring(0, 2));
                    var belakang = a % 1000;
                    kalimat = Terbilang(depan) + " Ribu " + Terbilang(belakang);
                }
                // 100,000 - 999,999
                else if (a < 1000000)
                {
                    var utama = a / 1000;
                    var depan = Convert.ToInt32(utama.ToString().Substring(0, 3));
                    var belakang = a % 1000;
                    kalimat = Terbilang(depan) + " Ribu " + Terbilang(belakang);
                }
                // 1,000,000 - 	99,999,999
                else if (a < 100000000)
                {
                    var utama = a / 1000000;
                    var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));//Substring(0, 4));
                    var belakang = a % 1000000;
                    kalimat = Terbilang(depan) + " Juta " + Terbilang(belakang);
                }
                else if (a < 1000000000)
                {
                    var utama = a / 1000000;
                    var depan = Convert.ToInt32(utama.ToString().Substring(0, 4));
                    var belakang = a % 1000000;
                    kalimat = Terbilang(depan) + " Juta " + Terbilang(belakang);
                }
                else if (a < 10000000000)
                {
                    var utama = a / 1000000000;
                    var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                    var belakang = a % 1000000000;
                    kalimat = Terbilang(depan) + " Milyar " + Terbilang(belakang);
                }
                else if (a < 100000000000)
                {
                    var utama = a / 1000000000;
                    var depan = Convert.ToInt32(utama.ToString().Substring(0, 2));
                    var belakang = a % 1000000000;
                    kalimat = Terbilang(depan) + " Milyar " + Terbilang(belakang);
                }
                else if (a < 1000000000000)
                {
                    var utama = a / 1000000000;
                    var depan = Convert.ToInt32(utama.ToString().Substring(0, 3));
                    var belakang = a % 1000000000;
                    kalimat = Terbilang(depan) + " Milyar " + Terbilang(belakang);
                }
                else if (a < 10000000000000)
                {
                    var utama = a / 10000000000;
                    var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                    var belakang = a % 10000000000;
                    kalimat = Terbilang(depan) + " Triliun " + Terbilang(belakang);
                }
                else if (a < 100000000000000)
                {
                    var utama = a / 1000000000000;
                    var depan = Convert.ToInt32(utama.ToString().Substring(0, 2));
                    var belakang = a % 1000000000000;
                    kalimat = Terbilang(depan) + " Triliun " + Terbilang(belakang);
                }

                else if (a < 1000000000000000)
                {
                    var utama = a / 1000000000000;
                    var depan = Convert.ToInt32(utama.ToString().Substring(0, 3));
                    var belakang = a % 1000000000000;
                    kalimat = Terbilang(depan) + " Triliun " + Terbilang(belakang);
                }

                else if (a < 10000000000000000)
                {
                    var utama = a / 1000000000000000;
                    var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                    var belakang = a % 1000000000000000;
                    kalimat = Terbilang(depan) + " Kuadriliun " + Terbilang(belakang);
                }

                var pisah = kalimat.Split(' ');
                List<string> full = new List<string>();// = [];
                for (var i = 0; i < pisah.Length; i++)
                {
                    if (pisah[i] != "") { full.Add(pisah[i]); }
                }
                return CombineTerbilang(full.ToArray());// full.Concat(' '); .join(' ');
            }
            catch (Exception ex)
            {
                return "";
                throw ex;
            }

        }
        static string CombineTerbilang(string[] arr)
        {
            return string.Join(" ", arr);
        }

        //public string TERBILANG_FIX(double x)
        //{
        //    try
        //        String TERBILANG_FIX = "";
        //    {
        //        double tampung;
        //        string teks;
        //        string bagian;
        //        int i;
        //        bool tanda;

        //        double x = System.Convert.ToDouble(extract_comma(x, "depan"));

        //        var[] letak = new var[6];
        //        letak[1] = "RIBU ";
        //        letak[2] = "JUTA ";
        //        letak[3] = "MILYAR ";
        //        letak[4] = "TRILYUN ";

        //        if ((x < 0))
        //        {
        //            TERBILANG_FIX = "";
        //            return;
        //        }

        //        if ((x == 0))
        //        {
        //            TERBILANG_FIX = "NOL";
        //            return;
        //        }


        //        if ((x < 2000))
        //            tanda = true;

        //        teks = "";


        //        if ((x >= 1.0E+15))
        //        {
        //            TERBILANG_FIX = "NILAI TERLALU BESAR";
        //            return;
        //        }

        //        for (i = 4; i >= 1; i += -1)
        //        {
        //            tampung = Conversion.Int(x / (Math.Pow(10, (3 * i))));
        //            if ((tampung > 0))
        //            {
        //                bagian = ratusan(tampung, tanda);
        //                teks = teks + bagian + letak[i];
        //            }
        //            x = x - tampung * (Math.Pow(10, (3 * i)));
        //        }

        //        teks = teks + ratusan(x, false);

        //        TERBILANG_FIX = Strings.StrConv((teks + "RUPIAH").ToString(), VbStrConv.ProperCase);
        //    }
        //    catch (Exception ex)
        //    {
        //        return "";
        //    }
        //}
        //public string ratusan(double y, bool flag)
        //{
        //    try
        //    {
        //        double tmp;
        //        string bilang;
        //        string bag;
        //        int j;


        //        String hasil="";

        //        // y = System.Math.Round(y, 2)

        //        string[] angka = new string[10];

        //        angka[1] = "SE";
        //        angka[2] = "DUA ";
        //        angka[3] = "TIGA ";
        //        angka[4] = "EMPAT ";
        //        angka[5] = "LIMA ";
        //        angka[6] = "ENAM ";
        //        angka[7] = "TUJUH ";
        //        angka[8] = "DELAPAN ";
        //        angka[9] = "SEMBILAN ";



        //        string[] posisi = new string[3];
        //        posisi[1] = "PULUH ";
        //        posisi[2] = "RATUS ";
        //        bilang = "";

        //        for (j = 2; j >= 1; j += -1)
        //        {
        //            int tmp = 0;
        //            tmp= y / (Math.Pow(10, j));
        //            if ((tmp > 0))
        //            {
        //                bag = angka[tmp];
        //                if ((j == 1 & tmp == 1))
        //                {
        //                    y = y - tmp * Math.Pow(10, j);
        //                    if ((y >= 1))
        //                        posisi[j] = "BELAS ";
        //                    else
        //                        angka[y] = "SE";
        //                    bilang = bilang + angka[y] + posisi[j];
        //                    ratusan = bilang;
        //                    return;
        //                }
        //                else
        //                    bilang = bilang + bag + posisi[j];
        //            }
        //            y = y - tmp * Math.Pow(10, j);
        //        }



        //        if ((flag == false))
        //            angka[1] = "SATU ";

        //        bilang = bilang + angka[y];
        //        hasil = bilang;
        //        return hasil;
        //    }
        //    catch (Exception ex)
        //    {
        //        return "";
        //    }
        //}

        public string extract_comma(string par_input, string par_opsi)
        {
            string _temp = "";
            if (par_input.Contains(",") | par_input.Contains("."))
            {
                int _posisi_koma;
                _posisi_koma = par_input.IndexOf(",");

                if (_posisi_koma == 0)
                    _posisi_koma = par_input.IndexOf(".");

                if (par_opsi.ToLower() == "depan")
                    _temp = par_input.Substring(0, _posisi_koma);
                else
                    _temp = par_input.Substring(_posisi_koma + 1, 2);
            }
            else
                _temp = par_input;

            return _temp;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sSql"></param>
        /// <returns></returns>
        public DataTable GetTableData(string sSql)
        {


            NpgsqlConnection DbConn;
            DbConn = new NpgsqlConnection(modClass.conString);
            NpgsqlCommand cmd;
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();

            try
            {
                // open the command objects connection
                DataTable Dt = new DataTable();

                DbConn.Open();

                cmd = new NpgsqlCommand();
                cmd.Connection = DbConn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSql;

                adapter.SelectCommand = cmd;
                adapter.Fill(Dt);


                return Dt;


            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                // cmd.Connection.Close()
                DbConn.Close();
                adapter.Dispose();
            }
        }

        public DataRow GetRowInfo(string sSql)
        {

            DataTable Dt = new DataTable();

            try
            {
                Dt = GetTableData(sSql);
                if (Dt.Rows.Count == 0)
                    return null;
                else if (Dt.Rows.Count > 1)
                    return Dt.Rows[0];
                else
                    return Dt.Rows[0];
            }
            catch (Exception e)
            {
                return null;
                throw e;

            }



        }


    }
}
