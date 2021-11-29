import { Component, OnInit } from '@angular/core';
import { Question } from '../models/question';
import { Round } from '../models/round';

@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css']
})
export class QuestionComponent implements OnInit {
  question: Question;
  constructor() {
    this.question = { round: Round.One, number: 1, category: 'Loading...', text: 'Loading...' } as Question;
  }

  ngOnInit(): void {
  }

}
