import { Component, OnInit } from "@angular/core";

@Component({
    selector: 'app-upload-files',
    templateUrl: './upload-files.component.html'
})

export class UploadFilesComponent implements OnInit {

    selectedFiles: File[] = [];

    constructor() { }

    ngOnInit(): void { }

    onFilesSelected(event) {
        var newFiles: File[] = <File[]>event.target.files;
        if (newFiles.length != 0) {
            this.selectedFiles = <File[]>event.target.files;
        }
    }

    onUpload() {

    }

    onClear() {
        this.selectedFiles = [];
    }
}