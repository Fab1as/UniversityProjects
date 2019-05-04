using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class CalcForm : Form
    {
        private BigInteger value { get; set; } = 0;
        private string operation { get; set; } = "";
        private bool operationPressed { get; set; } = false;
        private bool isFirst { get; set; } = true;

        

        public CalcForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button_Click(object sender, EventArgs e)
        {
            if (result.Text == "0" || operationPressed)
            {
                result.Clear();
            }
            operationPressed = false;
            Button button = (Button)sender;
            result.Text += button.Text;
        }

        private void buttonEqual_Click(object sender, EventArgs e)
        {
            if (!operationPressed)
            {
                updateValue();
            }
            equation.Text = "";
            result.Text = value.ToString();
            operation = "";
            operationPressed = false;
            isFirst = true;
        }

        private void buttonCE_Click(object sender, EventArgs e)
        {
            result.Text = "0";
        }

        private void buttonC_Click(object sender, EventArgs e)
        {
            value = 0;
            result.Text = "0";
            equation.Text = "";
            operationPressed = false;
        }

        private void operator_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (isFirst)
            {
                operation = button.Text;
                value = result.Text;
                isFirst = false;
            }
            else
            {
                updateValue();
                result.Text = value.ToString();
                operation = button.Text;
            }
            operationPressed = true;
            equation.Text = $"{value} {operation}";
        }

        private void updateValue()
        {
            switch (operation)
            {
                case "+":
                    value += result.Text;
                    break;
                case "-":
                    value -= result.Text;
                    break;
                case "*":
                    value *= result.Text;
                    break;
                case "/":
                    value /= result.Text;
                    break;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
