using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;

namespace Clicker;

public class Form1 : Form
{
	private const uint MOUSEEVENTF_LEFTDOWN = 2u;

	private const uint MOUSEEVENTF_LEFTUP = 4u;

	private static IKeyboardMouseEvents globalHook;

	private bool isActive;

	private bool isCapturingKey;

	private Keys bindedKey = Keys.F6;

	private IContainer components;

	private TextBox intervalTextBox;

	private Label label1;

	private Label bindedButtonLabel;

	private Button rebindButton;

	private TableLayoutPanel tableLayoutPanel1;

	[DllImport("user32.dll")]
	private static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, UIntPtr dwExtraInfo);

	public Form1()
	{
		InitializeComponent();
		KeyDown += MainForm_KeyDown;
		globalHook = Hook.GlobalEvents();
		globalHook.KeyDown += GlobalHook_KeyDown;
	}

	private void GlobalHook_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == bindedKey)
		{
			isActive = !isActive;
			if (isActive)
			{
				StartClickerLoop();
			}
		}
	}

	private async Task StartClickerLoop()
	{
		while (isActive)
		{
			mouse_event(2u, 0, 0, 0u, UIntPtr.Zero);
			mouse_event(4u, 0, 0, 0u, UIntPtr.Zero);
			int num = ((intervalTextBox.Text != "") ? int.Parse(intervalTextBox.Text) : 1000);
			num = ((num < 10) ? 10 : num);
			await Task.Delay(num);
		}
	}

	private void rebindButton_Click(object sender, EventArgs e)
	{
		isCapturingKey = true;
		bindedButtonLabel.Text = "Waiting for the button press";
	}

	private void MainForm_KeyDown(object sender, KeyEventArgs e)
	{
		if (isCapturingKey)
		{
			bindedKey = e.KeyCode;
			isCapturingKey = false;
			bindedButtonLabel.Text = "Binded button to switch the autocliker: " + bindedKey;
		}
	}

	private void intervalTextBox_TextChanged(object sender, EventArgs e)
	{
		int selectionStart = intervalTextBox.SelectionStart;
		string text = string.Concat(intervalTextBox.Text.Where(char.IsDigit));
		if (text != intervalTextBox.Text)
		{
			intervalTextBox.Text = text;
			intervalTextBox.SelectionStart = Math.Min(selectionStart, text.Length);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Clicker.Form1));
		this.intervalTextBox = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.bindedButtonLabel = new System.Windows.Forms.Label();
		this.rebindButton = new System.Windows.Forms.Button();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.tableLayoutPanel1.SuspendLayout();
		base.SuspendLayout();
		this.intervalTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom;
		this.tableLayoutPanel1.SetColumnSpan(this.intervalTextBox, 2);
		this.intervalTextBox.Location = new System.Drawing.Point(25, 29);
		this.intervalTextBox.Name = "intervalTextBox";
		this.intervalTextBox.Size = new System.Drawing.Size(229, 20);
		this.intervalTextBox.TabIndex = 0;
		this.intervalTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.intervalTextBox.TextChanged += new System.EventHandler(this.intervalTextBox_TextChanged);
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom;
		this.label1.AutoSize = true;
		this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
		this.label1.Location = new System.Drawing.Point(48, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(183, 26);
		this.label1.TabIndex = 1;
		this.label1.Text = "Click interval, ms (default value 1000)";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.bindedButtonLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.bindedButtonLabel.AutoSize = true;
		this.bindedButtonLabel.Location = new System.Drawing.Point(18, 47);
		this.bindedButtonLabel.Name = "bindedButtonLabel";
		this.bindedButtonLabel.Size = new System.Drawing.Size(118, 59);
		this.bindedButtonLabel.TabIndex = 2;
		this.bindedButtonLabel.Text = "Binded button to switch the autocliker: F6";
		this.bindedButtonLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.rebindButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.rebindButton.Location = new System.Drawing.Point(142, 50);
		this.rebindButton.MinimumSize = new System.Drawing.Size(89, 30);
		this.rebindButton.Name = "rebindButton";
		this.rebindButton.Size = new System.Drawing.Size(134, 53);
		this.rebindButton.TabIndex = 3;
		this.rebindButton.Text = "Rebind button";
		this.rebindButton.UseVisualStyleBackColor = true;
		this.rebindButton.Click += new System.EventHandler(this.rebindButton_Click);
		this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.tableLayoutPanel1.ColumnCount = 2;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel1.Controls.Add(this.rebindButton, 1, 2);
		this.tableLayoutPanel1.Controls.Add(this.intervalTextBox, 0, 1);
		this.tableLayoutPanel1.Controls.Add(this.bindedButtonLabel, 0, 2);
		this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
		this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
		this.tableLayoutPanel1.MinimumSize = new System.Drawing.Size(183, 51);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 3;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25f));
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55f));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(279, 106);
		this.tableLayoutPanel1.TabIndex = 4;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(303, 128);
		base.Controls.Add(this.tableLayoutPanel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		this.MinimumSize = new System.Drawing.Size(319, 167);
		base.Name = "Form1";
		this.Text = "AutoClicker";
		this.tableLayoutPanel1.ResumeLayout(false);
		this.tableLayoutPanel1.PerformLayout();
		base.ResumeLayout(false);
	}
}
