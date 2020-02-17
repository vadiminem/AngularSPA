import { Component, OnInit } from "@angular/core";
import { FileManagerService } from "src/app/shared/file-manager.service";

@Component({
  selector: 'app-upload-files',
  templateUrl: './upload-files.component.html',
  styleUrls: ['./upload-files.component.scss']
})

export class UploadFilesComponent implements OnInit {

  selectedFiles: File[] = [];

  constructor(private service: FileManagerService) { }

  ngOnInit(): void { }

  onFilesSelected(event) {
    var newFiles: File[] = <File[]>event.target.files;
    if (newFiles.length != 0) {
      this.selectedFiles = <File[]>event.target.files;
    }
  }

  onUpload() {
    const fd = new FormData();
    for (let i = 0; i < this.selectedFiles.length; i++) {
      fd.append('file', this.selectedFiles[i], this.selectedFiles[i].name);
    }

    this.service.filesForUpload = fd;
    this.service.postUploadFiles().subscribe(res => { this.service.refreshList(); this.onClear(); });
  }

  onClear() {
    this.selectedFiles = [];
  }
}
