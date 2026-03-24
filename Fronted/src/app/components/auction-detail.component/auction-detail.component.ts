import { Component, OnInit } from '@angular/core';
import { AuctionService } from '../../services/auction.service';
import { SignalRService } from '../../services/signalr.service';
import { ActivatedRoute } from '@angular/router';
import { Auction } from '../../Models/auction';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-auction-detail',
  templateUrl: './auction-detail.component.html',
  standalone: true,
  imports: [FormsModule,CommonModule]
})
export class AuctionDetailComponent implements OnInit {

  auction!: Auction;
  bidAmount: number = 0;

  constructor(
    private auctionService: AuctionService,
    private signalR: SignalRService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.auctionService.getAuction(id).subscribe(data => {
      this.auction = data;
    });

    this.signalR.startConnection();

    this.signalR.onBidUpdate((data) => {
      if (data.auctionId === id) {
        this.auction.currentPrice = data.price;
      }
    });
  }

  placeBid() {
    this.auctionService.placeBid(this.auction.id, {
      auctionId: this.auction.id,
      amount: this.bidAmount
    }).subscribe();
  }
}