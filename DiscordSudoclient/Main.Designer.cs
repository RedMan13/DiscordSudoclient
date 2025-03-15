namespace DiscordSudoclient
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            flpServers = new FlowLayoutPanel();
            flowLayoutPanel3 = new FlowLayoutPanel();
            flpMessages = new FlowLayoutPanel();
            flpChannels = new FlowLayoutPanel();
            pbSelectedIcon = new PictureBox();
            btnSendMessage = new Button();
            txtMessage = new TextBox();
            flpServers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbSelectedIcon).BeginInit();
            SuspendLayout();
            // 
            // flpServers
            // 
            flpServers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            flpServers.AutoScroll = true;
            flpServers.Controls.Add(flowLayoutPanel3);
            flpServers.Location = new Point(12, 59);
            flpServers.Name = "flpServers";
            flpServers.Size = new Size(62, 332);
            flpServers.TabIndex = 1;
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            flowLayoutPanel3.Location = new Point(3, 3);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new Size(41, 0);
            flowLayoutPanel3.TabIndex = 2;
            // 
            // flpMessages
            // 
            flpMessages.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flpMessages.AutoScroll = true;
            flpMessages.FlowDirection = FlowDirection.TopDown;
            flpMessages.Location = new Point(248, 12);
            flpMessages.Name = "flpMessages";
            flpMessages.Size = new Size(643, 350);
            flpMessages.TabIndex = 2;
            flpMessages.WrapContents = false;
            // 
            // flpChannels
            // 
            flpChannels.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            flpChannels.AutoScroll = true;
            flpChannels.Location = new Point(80, 12);
            flpChannels.Name = "flpChannels";
            flpChannels.Size = new Size(162, 379);
            flpChannels.TabIndex = 3;
            // 
            // pbSelectedIcon
            // 
            pbSelectedIcon.Location = new Point(20, 12);
            pbSelectedIcon.Name = "pbSelectedIcon";
            pbSelectedIcon.Size = new Size(41, 41);
            pbSelectedIcon.SizeMode = PictureBoxSizeMode.Zoom;
            pbSelectedIcon.TabIndex = 4;
            pbSelectedIcon.TabStop = false;
            // 
            // btnSendMessage
            // 
            btnSendMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSendMessage.AutoSize = true;
            btnSendMessage.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnSendMessage.Location = new Point(848, 368);
            btnSendMessage.Name = "btnSendMessage";
            btnSendMessage.Size = new Size(43, 25);
            btnSendMessage.TabIndex = 5;
            btnSendMessage.Text = "Send";
            btnSendMessage.UseVisualStyleBackColor = true;
            btnSendMessage.Click += btnSendMessage_Click;
            // 
            // txtMessage
            // 
            txtMessage.Location = new Point(248, 369);
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(594, 23);
            txtMessage.TabIndex = 6;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(903, 403);
            Controls.Add(txtMessage);
            Controls.Add(btnSendMessage);
            Controls.Add(pbSelectedIcon);
            Controls.Add(flpMessages);
            Controls.Add(flpChannels);
            Controls.Add(flpServers);
            MinimumSize = new Size(389, 104);
            Name = "Main";
            Text = "Form1";
            flpServers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbSelectedIcon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TreeView treeView1;
        private FlowLayoutPanel flpServers;
        private FlowLayoutPanel flpMessages;
        private FlowLayoutPanel flowLayoutPanel3;
        private FlowLayoutPanel flpChannels;
        private PictureBox pbSelectedIcon;
        private Button btnSendMessage;
        private TextBox txtMessage;
    }
}
