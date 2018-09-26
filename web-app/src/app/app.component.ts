import { Component, OnInit } from '@angular/core';
import { DataService } from './shared/data.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'web-app';

  constructor(private dataService: DataService) {

  }

  ngOnInit() {
    console.log("Inside app component");
    this.dataService.get('')
      .subscribe(response => {
        console.log(response);
      });
  }

}
