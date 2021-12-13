import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { HostCreateComponent } from './host/host-create/host-create.component';
import { PlayManagerComponent } from './play/play-manager/play-manager.component';
import { HostQuestionEditorComponent } from './host/host-question-editor/host-question-editor.component';
import { HomeComponent } from './home/home.component';
import { HostManageComponent } from './host/host-manage/host-manage.component';
import { PlayJoinComponent } from './play/play-join/play-join.component';

const routes: Routes = [
  {path: "", component: HomeComponent},
  {path: "host-setup", component: HostCreateComponent},
  {path: "host-edit", component: HostQuestionEditorComponent},
  {path: "host-play", component: HostManageComponent},
  {path: "play-setup", component: PlayJoinComponent},
  {path: "play", component: PlayManagerComponent}
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
