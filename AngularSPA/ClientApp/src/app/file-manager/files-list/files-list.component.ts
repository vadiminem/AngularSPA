import { Component, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Component({
    selector: 'app-files-list',
    templateUrl: './files-list.component.html'
})

export class FilesListComponent implements OnInit {

    constructor(private service: HttpClient) { }

    ngOnInit(): void { }
}