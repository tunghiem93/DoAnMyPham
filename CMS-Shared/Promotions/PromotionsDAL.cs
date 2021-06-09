using CMS_DTO.Promotions;
using CMS_Entity;
using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Shared.Promotions
{
    public class PromotionsDAL
    {
        public List<PromotionDTO> GetPromotions()
        {
            var models = new List<PromotionDTO>();
            using(var context = new CMS_Context())
            {
                models = context.Promotions.Where(z => z.IsDeleted == false).Select(z => new PromotionDTO
                {
                    Id = z.Id,
                    PromotionCode = z.PromotionCode,
                    Value = z.Value,
                    IsActive = z.IsActive
                }).ToList();
            }
            return models;
        }

        public bool CreatePromotion(PromotionDTO model)
        {
            var Result = false;
            try
            {
                using(var context = new CMS_Context())
                {
                    var Entity = new PromotionEntities
                    {
                        Id = Guid.NewGuid().ToString(),
                        PromotionCode = model.PromotionCode,
                        IsActive = model.IsActive,
                        CreatedBy = model.CreatedBy,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        Value = model.Value
                    };
                    context.Promotions.Add(Entity);
                    Result = context.SaveChanges() > 0;
                }
            }
            catch(Exception ex) { }
            return Result;
        }

        public bool Delete(string Id)
        {
            var Result = false;
            try
            {
                using(var context = new CMS_Context())
                {
                    var Entity = context.Promotions.Find(Id);
                    Entity.IsDeleted = true;
                    Entity.IsActive = false;

                    Result = context.SaveChanges() > 0;
                }
            }
            catch(Exception ex) { }
            return Result;
        }
    }
}
