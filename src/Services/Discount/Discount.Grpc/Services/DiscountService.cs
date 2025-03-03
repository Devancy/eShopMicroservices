using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
{
	public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
	{
		Coupon? coupon = null;
		if (!string.IsNullOrWhiteSpace(request?.ProductName))
			coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
		
		coupon ??= new Coupon { ProductName = "No discount", Description = "No discount available", Amount = 0 };
		var couponModel = coupon.Adapt<CouponModel>();
		
		logger.LogInformation("Get discount for : {ProductName}", couponModel.ProductName);
		
		return couponModel;
	}

	public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
	{
		var coupon = request.Coupon.Adapt<Coupon>() ?? throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));
		dbContext.Coupons.Add(coupon);
		await dbContext.SaveChangesAsync();
		
		logger.LogInformation("Create discount for : {ProductName}", coupon.ProductName);
		
		var couponModel = coupon.Adapt<CouponModel>();
		return couponModel;
			
	}

	public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
	{
		var coupon = request.Coupon.Adapt<Coupon>() ?? throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));
		dbContext.Coupons.Update(coupon);
		await dbContext.SaveChangesAsync();
		
		logger.LogInformation("Update discount for : {ProductName}", coupon.ProductName);
		
		var couponModel = coupon.Adapt<CouponModel>();
		return couponModel;
	}

	public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
	{
		var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
		if (coupon is null)
			throw new RpcException(new Status(StatusCode.NotFound, $"Discount not found for product={request.ProductName}"));
		dbContext.Coupons.Remove(coupon);
		await dbContext.SaveChangesAsync();
		
		logger.LogInformation("Delete discount for : {ProductName}", coupon.ProductName);
		
		return new DeleteDiscountResponse { Success = true };
	}
}