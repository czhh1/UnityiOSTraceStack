#include <stdio.h>
#include <execinfo.h>

extern "C"{
    void NativeCrash(){
        char *tmp = NULL;
        sprintf( tmp , "crash!!!!" );
    }
    void ObjCCrash(){
        [[NSArray array] objectAtIndex:0];
    }
    static char *global_backtrace_buffer = NULL;

    char* GetStackSymbols(){
        if( global_backtrace_buffer ){
            free(global_backtrace_buffer);
            global_backtrace_buffer = NULL;
        }
        void *addr[64];
        int nframes = backtrace(addr, sizeof(addr)/sizeof(*addr));
        if (nframes > 1) {
            char **syms = backtrace_symbols(addr, nframes);
            int bufSize = 0;
            for(int i=0;i<nframes;++i){
                bufSize += strlen(syms[i] ) + 1;
            }
            global_backtrace_buffer = (char*)malloc( bufSize );
            char *strPtr = global_backtrace_buffer;
            for(int i=0;i<nframes;++i){
                strncpy(strPtr,syms[i],strlen(syms[i]) );
                strPtr[ strlen(syms[i])] = '\n';
                
                strPtr += strlen(syms[i]) + 1;
            }
            global_backtrace_buffer[ bufSize -1] = '\0';
            NSLog(@"stackTrace:\n%s", global_backtrace_buffer );
            free(syms);
        } else {
            NSLog(@"%s: *** Failed to generate backtrace.", __func__);
        }
        return global_backtrace_buffer;
    }

}
