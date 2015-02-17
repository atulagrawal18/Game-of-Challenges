namespace WindowsFormsApplication1
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.txtVideo = new System.Windows.Forms.TextBox();
            this.btnOpenVideo = new System.Windows.Forms.Button();
            this.btnCreateVideo = new System.Windows.Forms.Button();
            this.btnAudioFile = new System.Windows.Forms.Button();
            this.txtAudio = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSubtitleFile = new System.Windows.Forms.Button();
            this.txtSubtitle = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Output Video File";
            // 
            // fontDialog1
            // 
            this.fontDialog1.Color = System.Drawing.SystemColors.ControlText;
            // 
            // txtVideo
            // 
            this.txtVideo.Location = new System.Drawing.Point(161, 131);
            this.txtVideo.Name = "txtVideo";
            this.txtVideo.Size = new System.Drawing.Size(283, 20);
            this.txtVideo.TabIndex = 5;
            // 
            // btnOpenVideo
            // 
            this.btnOpenVideo.Location = new System.Drawing.Point(460, 131);
            this.btnOpenVideo.Name = "btnOpenVideo";
            this.btnOpenVideo.Size = new System.Drawing.Size(116, 23);
            this.btnOpenVideo.TabIndex = 6;
            this.btnOpenVideo.Text = "Open Video File";
            this.btnOpenVideo.UseVisualStyleBackColor = true;
            this.btnOpenVideo.Click += new System.EventHandler(this.btnOpenVideo_Click);
            // 
            // btnCreateVideo
            // 
            this.btnCreateVideo.Location = new System.Drawing.Point(58, 183);
            this.btnCreateVideo.Name = "btnCreateVideo";
            this.btnCreateVideo.Size = new System.Drawing.Size(106, 23);
            this.btnCreateVideo.TabIndex = 7;
            this.btnCreateVideo.Text = "Create Video";
            this.btnCreateVideo.UseVisualStyleBackColor = true;
            this.btnCreateVideo.Click += new System.EventHandler(this.btnCreateVideo_Click);
            // 
            // btnAudioFile
            // 
            this.btnAudioFile.Location = new System.Drawing.Point(460, 34);
            this.btnAudioFile.Name = "btnAudioFile";
            this.btnAudioFile.Size = new System.Drawing.Size(116, 23);
            this.btnAudioFile.TabIndex = 10;
            this.btnAudioFile.Text = "Open Audio File";
            this.btnAudioFile.UseVisualStyleBackColor = true;
            this.btnAudioFile.Click += new System.EventHandler(this.btnAudioFile_Click);
            // 
            // txtAudio
            // 
            this.txtAudio.Location = new System.Drawing.Point(161, 36);
            this.txtAudio.Name = "txtAudio";
            this.txtAudio.Size = new System.Drawing.Size(283, 20);
            this.txtAudio.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(55, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Audio File";
            // 
            // btnSubtitleFile
            // 
            this.btnSubtitleFile.Location = new System.Drawing.Point(460, 79);
            this.btnSubtitleFile.Name = "btnSubtitleFile";
            this.btnSubtitleFile.Size = new System.Drawing.Size(116, 23);
            this.btnSubtitleFile.TabIndex = 13;
            this.btnSubtitleFile.Text = "Open Subtitle File";
            this.btnSubtitleFile.UseVisualStyleBackColor = true;
            this.btnSubtitleFile.Click += new System.EventHandler(this.btnSubtitleFile_Click);
            // 
            // txtSubtitle
            // 
            this.txtSubtitle.Location = new System.Drawing.Point(161, 81);
            this.txtSubtitle.Name = "txtSubtitle";
            this.txtSubtitle.Size = new System.Drawing.Size(283, 20);
            this.txtSubtitle.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Subtitle File";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 469);
            this.Controls.Add(this.btnSubtitleFile);
            this.Controls.Add(this.txtSubtitle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnAudioFile);
            this.Controls.Add(this.txtAudio);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCreateVideo);
            this.Controls.Add(this.btnOpenVideo);
            this.Controls.Add(this.txtVideo);
            this.Controls.Add(this.label2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.TextBox txtVideo;
        private System.Windows.Forms.Button btnOpenVideo;
        private System.Windows.Forms.Button btnCreateVideo;
        private System.Windows.Forms.Button btnAudioFile;
        private System.Windows.Forms.TextBox txtAudio;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSubtitleFile;
        private System.Windows.Forms.TextBox txtSubtitle;
        private System.Windows.Forms.Label label4;
    }
}

