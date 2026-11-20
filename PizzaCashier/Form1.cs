namespace PizzaCashier
{
    public partial class Form1 : Form
    {
        private readonly List<OrderItem> items = new List<OrderItem>();

        private ComboBox cmbPizza;
        private RadioButton rbSmall, rbMedium, rbFamily;
        private CheckBox chkCheese, chkBacon, chkMushrooms, chkOlives, chkHalapenio, chkCheddar;
        private TextBox txtQuantity;
        private Button btnAdd, btnSummary, btnNew;
        private ListBox lstOrder;
        private Label lblSummary;

        public Form1() {
            BuildUi();
        }

        private void BuildUi()
        {
            SuspendLayout();
            Text = "Cashier system for Pizza orders - Bohdan Rusakov F117156";
            ClientSize = new Size(980, 470);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Font = new Font("Segoe UI", 9.5F);
            BackColor = Color.FromArgb(255, 240, 235);

            Controls.Add(new Label { Text = "Pizza:", Location = new Point(20, 23), AutoSize = true });
            cmbPizza = new ComboBox
            {
                Location = new Point(150, 20),
                Width = 220,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            for (int i = 0; i < PizzaCalculator.PizzaNames.Length; i++)
            {
                cmbPizza.Items.Add($"{PizzaCalculator.PizzaNames[i]} - {PizzaCalculator.PizzaPrices[i]:F2} eur.");
            }
            cmbPizza.SelectedIndex = 0;
            Controls.Add(cmbPizza);
            //Size
            GroupBox grpSize = new GroupBox
            {
                Text = "Size",
                Location = new Point(20, 58),
                Size = new Size(350, 58)
            };
            rbSmall = new RadioButton
            {
                Text = "Small",
                AutoSize = true,
                Checked = true
            };
            rbMedium = new RadioButton
            {
                Text = "Medium (+25%)",
                AutoSize = true
            };
            rbFamily = new RadioButton
            {
                Text = "Family (+50%)",
                AutoSize = true
            };

            FlowLayoutPanel flowSize = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                WrapContents = false,
            };
            flowSize.Controls.AddRange(new Control[] { rbSmall, rbMedium, rbFamily });

            grpSize.Controls.Add(flowSize);
            Controls.Add(grpSize);


            //Pizza toppings
            GroupBox grpExtras = new GroupBox { Text = "Toppings (+1.55 eur. each)", Location = new Point(20, 124), Size = new Size(350, 110) };
            chkCheese = new CheckBox { Text = "Double cheese", Location = new Point(12, 24), AutoSize = true };
            chkBacon = new CheckBox { Text = "Bacon", Location = new Point(190, 24), AutoSize = true };
            chkMushrooms = new CheckBox { Text = "Mushrooms", Location = new Point(12, 52), AutoSize = true };
            chkOlives = new CheckBox { Text = "Olives", Location = new Point(190, 52), AutoSize = true };
            chkHalapenio = new CheckBox { Text = "Halapenio", Location = new Point(12, 80), AutoSize = true };
            chkCheddar = new CheckBox { Text = "Cheddar", Location = new Point(190, 80), AutoSize = true };
            grpExtras.Controls.AddRange(new Control[] { chkCheese, chkBacon, chkMushrooms, chkOlives, chkHalapenio, chkCheddar });
            Controls.Add(grpExtras);

            //quantity of pizzas

            Controls.Add(new Label { Text = "Quantity of pizzas: ", Location = new Point(20, 253), AutoSize = true });
            txtQuantity = new TextBox { Location = new Point(170, 250), Width = 80, Text = "1" };
            Controls.Add(txtQuantity);


            //Buttons
            btnAdd = new Button { Text = "Add to bill", Location = new Point(20, 288), Size = new Size(170, 38) };
            btnSummary = new Button { Text = "Close bill /Summary", Location = new Point(200, 288), Size = new Size(170, 38) };
            btnNew = new Button { Text = "New Bill", Location = new Point(20, 334), Size = new Size(350, 30) };
            Controls.AddRange(new Control[] { btnAdd, btnSummary, btnNew });

            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.BackColor = Color.FromArgb(200, 40, 40);
            btnAdd.ForeColor = Color.White;

            btnSummary.FlatStyle = FlatStyle.Flat;
            btnSummary.BackColor = Color.FromArgb(46, 125, 50);
            btnSummary.ForeColor = Color.White;

            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.BackColor = Color.FromArgb(240, 230, 210);

            AcceptButton = btnAdd;

            //Bills
            Controls.Add(new Label { Text = "Bill:", Location = new Point(390, 0), AutoSize = true });
            lstOrder = new ListBox { Location = new Point(390,22), Size = new Size(570,342), HorizontalScrollbar = true };
            Controls.Add(lstOrder);
            lstOrder.BackColor = Color.White;

            //Summary
            lblSummary = new Label
            {   
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(255, 236, 200),
                ForeColor = Color.FromArgb(120, 40, 0),
                Location = new Point(20, 378),
                Size = new Size(940, 80),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(6),
                Text = "Press \"Close bill / Summary\"."
            };
            Controls.Add(lblSummary);

            btnAdd.Click += BtnAdd_Click;

            btnSummary.Click += BtnSummary_Click;

            btnNew.Click += BtnNew_Click;

            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ResumeLayout(false);
            PerformLayout();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if(!int.TryParse(txtQuantity.Text.Trim(), out int  quantity) || quantity <= 0)
            {
                MessageBox.Show("Quantity must be positive whole number",
                    "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                txtQuantity.SelectAll();
                return;
            }

            int sizeIndex = rbMedium.Checked ? 1 : rbFamily.Checked ? 2 : 0;
            string sizeName = rbMedium.Checked ? "Medium" : rbFamily.Checked ? "Family" : "Small";

            List<string> extras = new List<string>();
            if (chkCheese.Checked) extras.Add("Double cheese");
            if (chkBacon.Checked) extras.Add("Bacon");
            if (chkMushrooms.Checked) extras.Add("Mushrooms");
            if (chkOlives.Checked) extras.Add("Olives");
            if (chkHalapenio.Checked) extras.Add("Halapenio");
            if (chkCheddar.Checked) extras.Add("Cheddar");

            OrderItem item = PizzaCalculator.CreateItem(cmbPizza.SelectedIndex, sizeIndex, sizeName, extras, quantity);

            items.Add(item);
            foreach(string line in item.ToLines())
            {
                lstOrder.Items.Add(line);
            }

            ClearToppings();
            txtQuantity.Text = "1";


        }

        private void BtnSummary_Click(object sender, EventArgs e)
        {
            if (items.Count == 0)
            {
                MessageBox.Show("The bill is empty.", "Summary",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            decimal totalSum = 0;
            int totalPizzaz = 0;
            OrderItem mostExpensive = items[0];

            foreach(OrderItem item in items)
            {
                totalSum += item.Total;
                totalPizzaz += item.Quantity;
                if(item.Total > mostExpensive.Total)
                {
                    mostExpensive = item;
                }
            }

            lblSummary.Text =
                $"Total to pay: {totalSum:F2} eur.{Environment.NewLine}" +
                $"Total pizzas ordered: {totalPizzaz}{Environment.NewLine}" +
                $"Most expensive item: {mostExpensive}";
            
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            items.Clear();
            lstOrder.Items.Clear();
            cmbPizza.SelectedIndex = 0;
            rbSmall.Checked = true;
            ClearToppings();
            txtQuantity.Text = "1";
            lblSummary.Text = "Press \"Close bill / Summary\".";
        }

        private void ClearToppings () 
        {
            chkCheese.Checked = chkBacon.Checked = chkMushrooms.Checked = chkOlives.Checked
                = chkHalapenio.Checked = chkCheddar.Checked = false;
        }



    }
}
