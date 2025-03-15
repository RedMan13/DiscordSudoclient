namespace DiscordSudoclient
{
    partial class Message
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pbUser = new PictureBox();
            lblUsername = new Label();
            lblContent = new Label();
            ((System.ComponentModel.ISupportInitialize)pbUser).BeginInit();
            SuspendLayout();
            // 
            // pbUser
            // 
            pbUser.ImageLocation = "https://cdn.discordapp.com/embed/avatars/{index}embed/avatars/0";
            pbUser.Location = new Point(0, 0);
            pbUser.MaximumSize = new Size(41, 41);
            pbUser.MinimumSize = new Size(41, 41);
            pbUser.Name = "pbUser";
            pbUser.Size = new Size(41, 41);
            pbUser.SizeMode = PictureBoxSizeMode.StretchImage;
            pbUser.TabIndex = 1;
            pbUser.TabStop = false;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(47, 3);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(73, 15);
            lblUsername.TabIndex = 2;
            lblUsername.Text = "Deleted User";
            // 
            // lblContent
            // 
            lblContent.AutoSize = true;
            lblContent.Location = new Point(47, 26);
            lblContent.Name = "lblContent";
            lblContent.Size = new Size(0, 15);
            lblContent.TabIndex = 3;
            // 
            // Message
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Controls.Add(lblContent);
            Controls.Add(lblUsername);
            Controls.Add(pbUser);
            Name = "Message";
            Size = new Size(123, 44);
            ((System.ComponentModel.ISupportInitialize)pbUser).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pbUser;
        private Label lblUsername;
        private Label lblContent;
    }
}
