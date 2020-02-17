import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { FileManagerComponent } from './file-manager/file-manager.component';
import { UploadFilesComponent } from './file-manager/upload-files/upload-files.component';
import { FilesListComponent } from './file-manager/files-list/files-list.component';
import { FileManagerService } from './shared/file-manager.service';

@NgModule({
  declarations: [
    AppComponent,
    FileManagerComponent,
    UploadFilesComponent,
    FilesListComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule
  ],
  providers: [FileManagerService],
  bootstrap: [AppComponent]
})
export class AppModule { }
