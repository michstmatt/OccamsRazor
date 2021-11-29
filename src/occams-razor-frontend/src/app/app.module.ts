import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { PlayJoinComponent } from './play/play-join/play-join.component';
import { HomeComponent } from './home/home.component';
import { QuestionComponent } from './question/question.component';
import { AppRoutingModule } from './app-routing.module';
import { PlayAnswerComponent } from './play/play-answer/play-answer.component';
import { PlayManagerComponent } from './play/play-manager/play-manager.component';
import { PlayWaitComponent } from './play/play-wait/play-wait.component';
import { HostCreateComponent } from './host/host-create/host-create.component';
import { HostQuestionEditorComponent } from './host/host-question-editor/host-question-editor.component';
import { RoundConverter } from './databind/round-converter';
import { HostManageComponent } from './host/host-manage/host-manage.component';

@NgModule({
  declarations: [
    AppComponent,
    PlayJoinComponent,
    HomeComponent,
    QuestionComponent,
    PlayAnswerComponent,
    PlayManagerComponent,
    PlayWaitComponent,
    HostCreateComponent,
    HostQuestionEditorComponent,
    RoundConverter,
    HostManageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
