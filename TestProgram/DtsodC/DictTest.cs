using static TestProgram.Program;

namespace TestProgram;

static class DictTest
{
    static void Fill(Dictionary<string,long> dict){
        for(long i=0;i<100000;i++)
            dict.Add($"key__{i}",i);
    }
    static long Gett(Dictionary<string,long> dict){
        long r=0;
        for(long i=0;i<100000;i++)
            r=dict[$"key__{i}"];
        return r;
    }

    static public void Test(){
        Info.Log("c","--------------[DictTest]---------------");
        Dictionary<string,long> dict=new();
        LogOperationTime("fill",1,()=>Fill(dict));
        LogOperationTime("gett",1,()=>Gett(dict));
    }
}