using Microsoft.Data.SqlClient;
using SalesManagement.Common;
using SalesManagement.Model;
using System.Data;
using System.Text;

namespace SalesManagement
{
    public partial class TopForm : Form
    {
        //���b�Z�[�W�\���p�N���X�̃C���X�^���X��
        MessageDsp msg = new MessageDsp();

        public TopForm()
        {
            fncMessageImport();
            InitializeComponent();
        }

        private void buttonDivision_Click(object sender, EventArgs e)
        {
            fncDivisionForm();
        }

        private void ToolStripMenuItemDivisionM_Click(object sender, EventArgs e)
        {
            fncDivisionForm();
        }

        ///////////////////////////////
        //���\�b�h���FfncDivisionForm()
        //���@��   �F�Ȃ�
        //�߂�l   �F�Ȃ�
        //�@�@�\   �F�����}�X�^�̃t�H�[�����J��
        ///////////////////////////////
        private void fncDivisionForm()
        {
            //�t�H�[���𓧖���
            Opacity = 0;

            //frmMenu��frm�Ƃ������O�ŊJ��
            DivisionForm frmD = new DivisionForm();
            frmD.ShowDialog();

            //�J�����t�H�[������߂��Ă�����
            //���������������
            frmD.Dispose();
        }

        ///////////////////////////////
        //���\�b�h���FfncMessageImport()
        //���@��   �F�Ȃ�
        //�߂�l   �F�Ȃ�
        //�@�@�\   �F���b�Z�[�W�e�[�u�����m�F���f�[�^��
        //          �F���݂��Ă��Ȃ���΃C���|�[�g
        ///////////////////////////////
        private void fncMessageImport()
        {
            try
            {
                using (var context = new SalesContext())
                {
                    //DB��M_Message�e�[�u���f�[�^�L���`�F�b�N
                    //�f�[�^�����݂��Ă��Ȃ��΃f�[�^���C���|�[�g
                    int cntMsg = context.MMessages.Count();
                    if (cntMsg > 0)
                    {
                        return;
                    }
                }

                //�C���|�[�g����CSV�t�@�C���̎w��
                string csvpth = Path.Combine(Environment.CurrentDirectory, "Message.csv");

                //�f�[�^�e�[�u���̐ݒ�
                DataTable dt = new DataTable();
                dt.TableName = "M_Message";

                //csv�t�@�C���̓��e��DataTable��
                // �S�s�ǂݍ���
                var rows = File.ReadAllLines(csvpth, Encoding.GetEncoding("Shift-JIS")).Select(x => x.Split(','));
                // ��ݒ�
                dt.Columns.AddRange(rows.First().Select(s => new DataColumn(s)).ToArray());
                // �s�ǉ�
                foreach (var row in rows.Skip(1))
                {
                    dt.Rows.Add(row);
                }

                //DB�ڑ����̎擾
                var dbpth = System.Configuration.ConfigurationManager.ConnectionStrings["SalesContext"].ConnectionString;
                //DataTable�̓��e��DB�֒ǉ�
                using (var bulkCopy = new SqlBulkCopy(dbpth))
                {
                    bulkCopy.DestinationTableName = dt.TableName;
                    bulkCopy.WriteToServer(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void �I��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // �t�H�[�������m�F���b�Z�[�W�̕\��
            DialogResult result = msg.MsgDsp("M00001");

            if (result == DialogResult.OK)
            {
                // OK�̎��̏���
                Close();
            }
            else
            {
                // �L�����Z���̎��̏���
            }
        }

        private void TopForm_Activated(object sender, EventArgs e)
        {
            if (Opacity == 0)
            {
                Opacity = 1;
            }
        }
    }
}
