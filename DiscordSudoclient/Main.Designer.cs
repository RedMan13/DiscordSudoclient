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
            flpServers.SuspendLayout();
            SuspendLayout();
            // 
            // flpServers
            // 
            flpServers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            flpServers.Controls.Add(flowLayoutPanel3);
            flpServers.Location = new Point(12, 12);
            flpServers.Name = "flpServers";
            flpServers.Size = new Size(41, 379);
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
            flpMessages.Location = new Point(206, 12);
            flpMessages.Name = "flpMessages";
            flpMessages.Size = new Size(685, 379);
            flpMessages.TabIndex = 2;
            // 
            // flpChannels
            // 
            flpChannels.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            flpChannels.Location = new Point(59, 12);
            flpChannels.Name = "flpChannels";
            flpChannels.Size = new Size(141, 379);
            flpChannels.TabIndex = 3;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(903, 403);
            Controls.Add(flpMessages);
            Controls.Add(flpChannels);
            Controls.Add(flpServers);
            MinimumSize = new Size(389, 104);
            Name = "Main";
            Text = "Form1";
            flpServers.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TreeView treeView1;
        private FlowLayoutPanel flpServers;
        private FlowLayoutPanel flpMessages;
        private FlowLayoutPanel flowLayoutPanel3;
        private FlowLayoutPanel flpChannels;
    }
}
