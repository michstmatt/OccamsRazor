import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { PlayService } from 'src/app/services/play.service';

@Component({
  selector: 'app-play-join',
  templateUrl: './play-join.component.html',
  styleUrls: ['./play-join.component.css']
})
export class PlayJoinComponent implements OnInit {

  constructor(
    private formBuilder: FormBuilder,
    private playService: PlayService,
    private router: Router
  ) { }

  joinForm = this.formBuilder.group({
    name: '',
    code: '1981169330'
  });

  ngOnInit(): void {
  }

  async onSubmit(): Promise<void> {
    this.playService.joinGame(
      Number.parseInt(this.joinForm.value.code),
      { name: this.joinForm.value.name }
    ).then(() =>
      this.router.navigate(["/play"])
    );
  }

}
