using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Office.Interop.Word;

namespace LITPC
{
    class litapi
    {
        pb progbar;


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                 STORY                         /////////////////////////////////////////


        string author = "";
            string category = "";
            string description = "";
            int num_pages = 0;
            string story = "";
            string title = "";
            int currentpage = 1;
        bool cont = true;
        string saveloc = "";
            string url = "";
            string baseurl = "";
            HtmlDocument doc;
        public void seturl(string link)
        {
            url = link;
            progbar= new pb();
            progbar.SetProgress(0);
            
        }
        public void setLoc(string loc)
        {
            saveloc = loc;
        }

        public void init()
        {
          
            if (currentpage == 1)
            {
                baseurl = url;
                loadfile();
                if(cont)
                {
                    getauthor();
                    getcategory();
                    getDescription();
                    getnumpages();
                    getStory();
                    progbar.SetProgress(10);
                    gettitle();
                    progbar.SetProgress(20);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Eg. 'https://www.literotica.com/s/story-name'", "Error: Invalid URL.",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
               
               
            }
            else
            {
                loadfile();
                getStory();
            }
            
        }

        public void loadfile()
        {
            /*bool dev = false;
            if(dev)
            {
                doc = new HtmlDocument();
                doc.Load(path);
            }
            else
            {*/
               
                if(currentpage!=1)
                {
                    url =baseurl+ "?page=" + currentpage;
                }
            //var html = @"https://www.literotica.com/s/"+storyid;
            try
            {
                HtmlWeb web = new HtmlWeb();
                doc = web.Load(url);
                
                progbar.SetProgress(5);
                progbar.Show();

            }
            catch
            {
               
                cont = false;
            }
                
           // }


        }

        public void getauthor()
        {
            var node = doc.DocumentNode.SelectSingleNode("//div[@class='b-story-header']/span/a");
            var aut = HttpUtility.HtmlDecode(node.InnerText);
            author = aut;
        }

        public void getcategory()
        {
            var node = doc.DocumentNode.SelectSingleNode("//div[@class='b-breadcrumbs']/a");

            var cat = HttpUtility.HtmlDecode(node.InnerText);
            category = cat;
        }

        public void getDescription()
        {
            var node = doc.DocumentNode.SelectNodes("//meta[@name='description']");
            string content = "Unable to Retrieve Description.";
            foreach (var nodes in node)
            {
                content = nodes.GetAttributeValue("content","");
                
            }
            var desc = HttpUtility.HtmlDecode(content);
            description = desc;

        }

        public void getnumpages()
        {
            var node = doc.DocumentNode.SelectNodes("//div[@class='b-pager-pages']/form/select[@name='page']/option");
            int num=1;
            try
            {
                foreach (var nodes in node)
                {
                    num = int.Parse(nodes.GetAttributeValue("value", ""));
                }
            }
            catch { }
            
            num_pages = num;


        }

        public void getStory()
        {
            
                var node = doc.DocumentNode.SelectSingleNode("//div[@class='b-story-body-x x-r15']");
                var st = HttpUtility.HtmlDecode(node.InnerText);
                story += st;
            progbar.SetProgress(30);
            if (currentpage!=num_pages)
                {
                    nextpage();
                }
               
            
           
        }

        public void gettitle()
        {
            var node = doc.DocumentNode.SelectSingleNode("//div[@class='b-story-header']/h1");

            var ttl = HttpUtility.HtmlDecode(node.InnerText);
            title = ttl;

        }

        public void nextpage()
        {
            //?page = 2
            currentpage++;
            loadfile();
            getStory();
        }




            public void test()
        {
            StreamWriter File = new StreamWriter("path.txt");
            File.WriteLine("Author: " +author);
            File.WriteLine("\n Category: " + category);
            File.WriteLine("\n Desc: " + description);
            File.WriteLine("\n Pages: " + num_pages);
            File.WriteLine("\n Story: " + story);
            File.WriteLine("\n Title: " + title);
            File.Close();
        }

      
        public void savefile()
        {
            if (cont)
            {

            
            Microsoft.Office.Interop.Word.Application word = new Application();
            word.Visible = false;
                progbar.SetProgress(40);

                var doc = word.Documents.Add();
            //TITLE
            var head = doc.Content.Paragraphs.Add();
            object styleHeading1 = "Heading 1";
            head.Range.set_Style(styleHeading1);
            head.Range.Text = title;
            head.Range.InsertParagraphAfter();

            // Author
            var head2 = doc.Content.Paragraphs.Add();
            object styleHeading2 = "Heading 2";
            head2.Range.set_Style(styleHeading2);
            head2.Range.Text = author;
            head2.Range.InsertParagraphAfter();
                progbar.SetProgress(50);



                //Story
                var paragraph1 = doc.Content.Paragraphs.Add();

            paragraph1.Range.Font.Name = "Calibri";
            paragraph1.Range.Font.Size = 11;

            paragraph1.Range.ParagraphFormat.SpaceBefore = 30;
            paragraph1.Range.ParagraphFormat.SpaceAfter = 30;
            
            paragraph1.Range.Text = story;
            paragraph1.Range.InsertParagraphAfter();
                progbar.SetProgress(60);
                try
            {
                    string ft = "";
                    int l = title.Length;
                    for(int i=0;i<l; i++)
                    {
                        char c = title.ElementAt(i);
                        if(c.Equals('\\') || c.Equals('/') || c.Equals(':') || c.Equals('*') || c.Equals(':') || c.Equals('?') || c.Equals('"') || c.Equals('<') || c.Equals('>') || c.Equals('|'))
                        {
                            c = '-';
                        }
                        ft += c;
                    }
                doc.SaveAs2(saveloc+@"Downloads\" + ft + ".docx");
                    progbar.SetProgress(70);
                }
            catch
            {
               
                System.Windows.Forms.MessageBox.Show("Unable to Write to File. File Maybe Open in Another Program.", "Error: Access Denied.",
                      System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

            }
            
            

            word.Quit();
                progbar.SetProgress(100);
                progbar.Dispose();
                System.Windows.Forms.MessageBox.Show("Download Complete!");
            }
        }







        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///STORY///////////////////////////////////////////////////////////////////////////////////////////////////





    }
}
