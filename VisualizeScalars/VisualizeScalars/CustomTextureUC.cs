using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using VisualizeScalars.DataQuery;
using VisualizeScalars.Helpers;

namespace VisualizeScalars
{
    public partial class CustomTextureUC : UserControl
    {
        
        private DataGrid<BaseGridCell> DataGrid;
        public float[,] Grid;
        public int Radius => this.colorSelection1.Radius;
        public Color Color => this.colorSelection1.Color;
        public CustomTextureUC(DataGrid<BaseGridCell> dataGrid)
        {
            this.DataGrid = dataGrid;
            InitializeComponent();
            cbxComparer.SelectedIndex = 0;
        }

        private void cmdCalculate_Click(object sender, EventArgs e)
        {
            if (tbxLeftHand.Text == "" && tbxRightHand.Text == "")
            {
                return;
            }

            float[,] resultGrid1;
            float[,] resultGrid2;
            switch (cbxComparer.SelectedIndex)
            {
                case 0:
                    Grid = CalculateGrid(tbxLeftHand.Text);
                    break;
                case 1:
                    Grid = CalculateGrid(tbxRightHand.Text);
                    break;
                case 2:
                    resultGrid1 = CalculateGrid(tbxLeftHand.Text);
                    resultGrid2 = CalculateGrid(tbxRightHand.Text);
                    Grid = new float[resultGrid1.GetLength(0), resultGrid1.GetLength(1)];
                    for (int j = 0; j < resultGrid1.GetLength(1); j++)
                        for (int i = 0; i < resultGrid1.GetLength(0); i++)
                        {
                            if (resultGrid1[i, j] > resultGrid2[i, j])
                            {
                                Grid[i, j] = 1;
                            }
                        }
                    break;
                case 3:
                    resultGrid1 = CalculateGrid(tbxLeftHand.Text);
                    resultGrid2 = CalculateGrid(tbxRightHand.Text);
                    Grid = new float[resultGrid1.GetLength(0), resultGrid1.GetLength(1)];
                    for (int j = 0; j < resultGrid1.GetLength(1); j++)
                    for (int i = 0; i < resultGrid1.GetLength(0); i++)
                    {
                        if (resultGrid1[i, j] < resultGrid2[i, j])
                        {
                            Grid[i, j] = 1;
                        }
                    }
                   
                    break;
                case 4:
                    resultGrid1 = CalculateGrid(tbxLeftHand.Text);
                    resultGrid2 = CalculateGrid(tbxRightHand.Text);
                    Grid = new float[resultGrid1.GetLength(0), resultGrid1.GetLength(1)];
                    for (int j = 0; j < resultGrid1.GetLength(1); j++)
                    for (int i = 0; i < resultGrid1.GetLength(0); i++)
                    {
                        if (resultGrid1[i, j] == resultGrid2[i, j])
                        {
                            Grid[i, j] = 1;
                        }
                    }
                    break;
            }

            this.colorSelection1.Text = "Calculated Texture";
            this.colorSelection1.Grid = Grid;
            Update();

        }

        private float[,] CalculateGrid(string mathExpression)
        {
            Regex maxFuncRegex = new Regex($"(?<func>max|min)\\(\\s*(?<property>\\w+)\\s*\\)");
            if (maxFuncRegex.IsMatch(mathExpression))
            {
                var properties = maxFuncRegex.Matches(mathExpression);
                foreach (Match match in properties)
                {
                    var property = match.Groups["property"].Value;
                    var func = match.Groups["func"].Value;
                    if (!DataGrid.PropertyNames.Contains(property))
                    {
                        return null;
                    } 
                    var text = match.Value;
                    double minMax = 0.0;
                    if (func == "min")
                    {
                        minMax = DataGrid.Min(property);
                    }
                    else if (func == "max")
                    {
                        minMax = DataGrid.Max(property);
                    }
                    
                    
                    mathExpression= mathExpression.Replace(text, minMax.ToString(CultureInfo.InvariantCulture));
                }
            }
            float[,] grid = new float[DataGrid.Width, DataGrid.Height];
            Regex regex = new Regex("(?<=(\\w+\\.?\\d*))\\s?([\\+\\-\\*\\/]{1})\\s?(?=(\\w+\\.?\\d*))");
            float fvalue;
            bool isFloat = float.TryParse(mathExpression,out fvalue);
            var lhValues = regex.Matches(mathExpression);
            if (lhValues.Count == 0 && !DataGrid.PropertyNames.Contains(mathExpression) && !isFloat)
                return null;
            if (DataGrid.PropertyNames.Contains(mathExpression))
                return DataGrid.GetDataGrid(mathExpression);
            else if(isFloat)
            {
                for (int j = 0; j < DataGrid.Height; j++)
                for (int i = 0; i < DataGrid.Width; i++)
                {
                    grid[i, j] = fvalue;
                }
                return grid;
            }
            var multiplications = lhValues.Where(m => m.Value.Trim() == "*").Select(x => x.Groups.Values.Where(gv => gv.Value.Trim() != "*").ToArray());
            Multiply(multiplications,ref grid, ref mathExpression);
            lhValues = regex.Matches(mathExpression);
            var divisions = lhValues.Where(m => m.Value.Trim() == "/").Select(x => x.Groups.Values.Where(gv => gv.Value.Trim() != "/").ToArray());
            Divide(divisions, ref grid, ref mathExpression);
            lhValues = regex.Matches(mathExpression);
            var additions = lhValues.Where(m => m.Value.Trim() == "+").Select(x => x.Groups.Values.Where(gv => gv.Value.Trim() != "+").ToArray());
            Add(additions, ref grid, ref mathExpression);
            lhValues = regex.Matches(mathExpression);
            var substractions = lhValues.Where(m => m.Value.Trim() == "-").Select(x => x.Groups.Values.Where(gv => gv.Value.Trim() != "-").ToArray());
            Substract(substractions,ref grid, ref mathExpression);
            return grid;
            
        } 
        private void Add(IEnumerable<Group[]> additions, ref float[,] grid, ref string expression)
        {
            float[,] resultGrid = new float[DataGrid.Width, DataGrid.Height];
            foreach (var addition in additions)
            {

                float valLh;
                var isFloatLH = float.TryParse(addition[0].Value, out valLh);
                float valRh;
                var isFloatRH = float.TryParse(addition[1].Value, out valRh);
                if (isFloatLH && isFloatRH)
                {
                    for (int j = 0; j < DataGrid.Height; j++)
                        for (int i = 0; i < DataGrid.Width; i++)
                        {
                            grid[i, j] = valLh + valRh;
                        }
                }
                else if (isFloatRH)
                {
                    float[,] scalarset;
                    if (addition[0].Value == "Result")
                        scalarset = grid;
                    else
                        scalarset = DataGrid.GetDataGrid(addition[0].Value);
                    for (int j = 0; j < DataGrid.Height; j++)
                        for (int i = 0; i < DataGrid.Width; i++)
                        {
                            grid[i, j] = scalarset[i, j] + valRh;
                        }
                }
                else if (isFloatLH)
                {
                    float[,] scalarset;
                    if (addition[1].Value == "Result")
                        scalarset = grid;
                    else
                        scalarset = DataGrid.GetDataGrid(addition[1].Value);
                    for (int j = 0; j < DataGrid.Height; j++)
                        for (int i = 0; i < DataGrid.Width; i++)
                        {
                            grid[i, j] = scalarset[i, j] + valLh;
                        }
                }
                else
                {
                    float[,] lhScalarset, rhScalarset;
                    if (addition[0].Value == "Result")
                        lhScalarset = grid;
                    else
                        lhScalarset = DataGrid.GetDataGrid(addition[0].Value);

                    if (addition[1].Value == "Result")
                        rhScalarset = grid;
                    else
                        rhScalarset = DataGrid.GetDataGrid(addition[1].Value);

                    for (int j = 0; j < DataGrid.Height; j++)
                        for (int i = 0; i < DataGrid.Width; i++)
                        {
                            grid[i, j] = lhScalarset[i, j] + rhScalarset[i, j];
                        }
                }
                Regex replaceRegex = new Regex($"{addition[0]}\\s?\\+\\s?{addition[1]}");
                expression = replaceRegex.Replace(expression, "Result");
            }

        }

        private void Substract(IEnumerable<Group[]> substractions, ref float[,] grid, ref string expression)
        {
            foreach (var substraction in substractions)
            {

                float valLh;
                var isFloatLH = float.TryParse(substraction[0].Value, out valLh);
                float valRh;
                var isFloatRH = float.TryParse(substraction[1].Value, out valRh);
                if (isFloatLH && isFloatRH)
                {
                    for (int j = 0; j < DataGrid.Height; j++)
                        for (int i = 0; i < DataGrid.Width; i++)
                        {
                            grid[i, j] = valLh - valRh;
                        }
                }
                else if (isFloatRH)
                {
                    float[,] scalarset;
                    if (substraction[0].Value == "Result")
                        scalarset = grid;
                    else
                        scalarset = DataGrid.GetDataGrid(substraction[0].Value);
                    for (int j = 0; j < DataGrid.Height; j++)
                        for (int i = 0; i < DataGrid.Width; i++)
                        {
                            grid[i, j] = scalarset[i, j] - valRh;
                        }
                }
                else if (isFloatLH)
                {
                    float[,] scalarset;
                    if (substraction[1].Value == "Result")
                        scalarset = grid;
                    else
                        scalarset = DataGrid.GetDataGrid(substraction[1].Value);
                    for (int j = 0; j < DataGrid.Height; j++)
                        for (int i = 0; i < DataGrid.Width; i++)
                        {
                            grid[i, j] = scalarset[i, j] - valLh;
                        }
                }
                else
                {
                    float[,] lhScalarset, rhScalarset;
                    if (substraction[0].Value == "Result")
                        lhScalarset = grid;
                    else
                        lhScalarset = DataGrid.GetDataGrid(substraction[0].Value);

                    if (substraction[1].Value == "Result")
                        rhScalarset = grid;
                    else
                        rhScalarset = DataGrid.GetDataGrid(substraction[1].Value);

                    for (int j = 0; j < DataGrid.Height; j++)
                        for (int i = 0; i < DataGrid.Width; i++)
                        {
                            grid[i, j] = lhScalarset[i, j] - rhScalarset[i, j];
                        }
                }
                Regex replaceRegex = new Regex($"{substraction[0]}\\s?\\-\\s?{substraction[1]}");
                expression = replaceRegex.Replace(expression, "Result");
            }
        }
        private void Divide(IEnumerable<Group[]> divisions, ref float[,] grid, ref string expression)
        {
           
            foreach (var division in divisions)
            {

                float valLh;
                var isFloatLH = float.TryParse(division[0].Value, out valLh);
                float valRh;
                var isFloatRH = float.TryParse(division[1].Value, out valRh);
                if (isFloatLH && isFloatRH)
                {
                    for (int j = 0; j < DataGrid.Height; j++)
                        for (int i = 0; i < DataGrid.Width; i++)
                        {
                            grid[i, j] = valLh / valRh;
                        }
                }
                else if (isFloatRH)
                {
                    float[,] scalarset;
                    if (division[0].Value == "Result")
                        scalarset = grid;
                    else
                        scalarset = DataGrid.GetDataGrid(division[0].Value);
                    for (int j = 0; j < DataGrid.Height; j++)
                        for (int i = 0; i < DataGrid.Width; i++)
                        {
                            grid[i, j] = scalarset[i, j] / valRh;
                        }
                }
                else if (isFloatLH)
                {
                    float[,] scalarset;
                    if (division[1].Value == "Result")
                        scalarset = grid;
                    else
                        scalarset = DataGrid.GetDataGrid(division[1].Value);
                    for (int j = 0; j < DataGrid.Height; j++)
                        for (int i = 0; i < DataGrid.Width; i++)
                        {
                            grid[i, j] = scalarset[i, j] / valLh;
                        }
                }
                else
                {
                    float[,] lhScalarset, rhScalarset;
                    if (division[0].Value == "Result")
                        lhScalarset = grid;
                    else
                        lhScalarset = DataGrid.GetDataGrid(division[0].Value);
                    
                    if (division[1].Value == "Result")
                        rhScalarset = grid;
                    else
                        rhScalarset = DataGrid.GetDataGrid(division[1].Value);
                    
                    for (int j = 0; j < DataGrid.Height; j++)
                        for (int i = 0; i < DataGrid.Width; i++)
                        {
                            grid[i, j] = lhScalarset[i, j] / rhScalarset[i, j];
                        }
                }
                Regex replaceRegex = new Regex($"{division[0]}\\s?\\/\\s?{division[1]}");
                expression = replaceRegex.Replace(expression, "Result");
            }
        }

        private void Multiply(IEnumerable<Group[]> multiplications,ref float[,] grid, ref string expression)
        {
            float[,] resultGrid = new float[DataGrid.Width,DataGrid.Height];
            foreach (var multiplication in multiplications)
            {

                float valLh;
                var isFloatLH = float.TryParse(multiplication[0].Value,NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture, out valLh);
                float valRh;
                var isFloatRH = float.TryParse(multiplication[1].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out valRh);
                if (isFloatLH && isFloatRH)
                {
                    for (int j = 0; j < DataGrid.Height; j++)
                    for (int i = 0; i < DataGrid.Width; i++)
                    {
                        grid[i, j] = valLh * valRh;
                    }
                }
                else if (isFloatRH)
                {
                    float[,] scalarset;
                    if (multiplication[0].Value == "Result")
                        scalarset = grid;
                    else
                        scalarset = DataGrid.GetDataGrid(multiplication[0].Value);
                    for (int j = 0; j < DataGrid.Height; j++)
                    for (int i = 0; i < DataGrid.Width; i++)
                    {
                        grid[i, j] = scalarset[i, j] * valRh;
                    }
                }
                else if (isFloatLH)
                {
                    float[,] scalarset;
                    if (multiplication[1].Value == "Result")
                        scalarset = grid;
                    else
                        scalarset = DataGrid.GetDataGrid(multiplication[1].Value);
                    
                    for (int j = 0; j < DataGrid.Height; j++)
                    for (int i = 0; i < DataGrid.Width; i++)
                    {
                        grid[i, j] = scalarset[i, j] * valLh;
                    }
                }
                else
                {
                    float[,] lhScalarset, rhScalarset;
                    if (multiplication[0].Value == "Result")
                        lhScalarset = grid;
                    else
                        lhScalarset = DataGrid.GetDataGrid(multiplication[0].Value);

                    if (multiplication[1].Value == "Result")
                        rhScalarset = grid;
                    else
                        rhScalarset = DataGrid.GetDataGrid(multiplication[1].Value);
                    for (int j = 0; j < DataGrid.Height; j++)
                    for (int i = 0; i < DataGrid.Width; i++)
                    {
                        grid[i, j] = lhScalarset[i, j] * rhScalarset[i, j];
                    }
                }
                Regex replaceRegex = new Regex($"{multiplication[0]}\\s?\\*\\s?{multiplication[1]}");
                expression = replaceRegex.Replace(expression, "Result");
            }
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            
            int top = 15;
            foreach (Control control in this.Parent.Controls)
            {
                if (control == this)
                    continue;
                control.Top = top;
                top += control.Height + 10;
            }
            this.Parent.Controls.Remove(this);
        }
    }
}
