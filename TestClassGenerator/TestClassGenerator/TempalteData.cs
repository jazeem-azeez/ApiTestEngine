using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClassGenerator
{
    public class TestGeneratorTemplate
    {
        public string FunctionDefPrefix { get; set; }
        public string FunctionDefPostfix { get; set; }
        public string ClassDefPostFix { get; set; }
        public string ClassDefPreFix { get; set; }
        public string FunctionBodySource { get; set; }
        public string ClassBodySource { get; set; }
    }
}
