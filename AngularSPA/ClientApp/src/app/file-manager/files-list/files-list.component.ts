import { Component, OnInit } from "@angular/core";
import { FileManagerService } from "src/app/shared/file-manager.service";
import { saveAs } from "file-saver";
import { formatDate } from "@angular/common";

@Component({
  selector: 'app-files-list',
  templateUrl: './files-list.component.html',
  styleUrls: ['./files-list.component.scss']
})

export class FilesListComponent implements OnInit {
  previousElement = null;

  previousParentElement = null;
  constructor(private service: FileManagerService) { }

  ngOnInit(): void {
    this.service.refreshList();
  }

  onClickListItem(event) {
    var parent = event.target
    var flag = true
    if (this.previousElement != null) {
      this.previousParentElement.classList.toggle('list-item-selected')
      for (let i = 0; i < this.previousElement.children.length; i++) {
        this.previousElement.children[i].classList.toggle('hidden')
      }
    }
    while (flag) {
      for (let i = 0; i < parent.classList.length; i++) {
        if (parent.classList[i] == 'list-item') {
          this.previousParentElement = parent
          parent.classList.toggle('list-item-selected')
          parent = parent.children[0].children[0];
          flag = false
          this.previousElement = parent;
          for (let i = 0; i < parent.children.length; i++) {
            parent.children[i].classList.toggle('hidden')
          }
        }
        else {
          parent = parent.parentElement
        }
      }


    }
  }

  onDownload(id, name) {
    let fileName = name;
    //file type extension
    let checkFileType = fileName.split('.').pop();
    var fileType;
    if (checkFileType == ".txt") {
      fileType = "text/plain";
    }
    if (checkFileType == ".pdf") {
      fileType = "application/pdf";
    }
    if (checkFileType == ".doc") {
      fileType = "application/vnd.ms-word";
    }
    if (checkFileType == ".docx") {
      fileType = "application/vnd.ms-word";
    }
    if (checkFileType == ".xls") {
      fileType = "application/vnd.ms-excel";
    }
    if (checkFileType == ".png") {
      fileType = "image/png";
    }
    if (checkFileType == ".jpg") {
      fileType = "image/jpeg";
    }
    if (checkFileType == ".jpeg") {
      fileType = "image/jpeg";
    }
    if (checkFileType == ".gif") {
      fileType = "image/gif";
    }
    if (checkFileType == ".csv") {
      fileType = "text/csv";
    }
    this.service.getDownloadFile(id)
      .subscribe(
        res => {
          var blob = new Blob([res], { type: fileType });
          saveAs(blob, fileName);
        });
  }

  onRemove(id) {
    this.service.deleteFileLink(id).subscribe(res => {
      this.service.refreshList();
    });
  }



}
