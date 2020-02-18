import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})

export class FileManagerService {
  readonly rootUrl = '/api/FileLinks';
  files = [];
  //linksList: FileLink[];
  linksList: {};
  keys: any[];
  filesForUpload: FormData;
  options = { headers: new HttpHeaders({ 'withCredentials': 'true' }) }


  constructor(private http: HttpClient) { }


  postUploadFiles() {
    return this.http.post(this.rootUrl, this.filesForUpload, this.options);
  }

  getDownloadFile(id) {
    return this.http.get(this.rootUrl + '/' + id,
      { headers: new HttpHeaders({ 'withCredentials': 'true' }), responseType: 'blob' });
  }

  deleteFileLink(id) {
    return this.http.delete(this.rootUrl + '/' + id, this.options);
  }


  refreshList() {
    this.http.get(this.rootUrl, this.options)
      .toPromise()
      .then(res => {
        //this.linksList = res as FileLink[];
        this.linksList = res;
        this.keys = Object.keys(res).reverse();
        console.log(this.keys)
      });
  }



}
