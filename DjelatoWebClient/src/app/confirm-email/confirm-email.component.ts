import { Component, OnInit } from '@angular/core';
import { IResponseContent } from '../shared/models/response-content';
import { MatDialog } from '@angular/material';
import { ConfirmEmailPopupComponent } from '../confirm-email-popup/confirm-email-popup.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.css']
})
export class ConfirmEmailComponent implements OnInit {

  constructor(
    private dialog: MatDialog,
    private toastr: ToastrService
  ) { }

  ngOnInit() { }
  
    openDialog(){
      const dialogRef =  this.dialog.open(ConfirmEmailPopupComponent, {
        autoFocus: false,
        disableClose: true
      });

      dialogRef.afterClosed().subscribe((result: IResponseContent)=>{

        if (result && result.isSucceeded){
          this.toastr.success(
            'Email has confirmed successfully', 
            'Notification!',             
          {
            positionClass: 'toast-top-full-width',
          });
        }
      });
    }
}
