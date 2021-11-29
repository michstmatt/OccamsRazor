import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-play-wait',
  templateUrl: './play-wait.component.html',
  styleUrls: ['./play-wait.component.css']
})
export class PlayWaitComponent implements OnInit {

  text: string
  constructor() {
    this.text = "";
  }

  ngOnInit(): void {
    setInterval(() => {
      this.text = this.text + ".";
      if(this.text.length > 3)
      {
        this.text = "";
      }
    }, 1000);
  }

}
