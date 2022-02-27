using Slalom_To_Do_Application.Database;
using System.Collections.Generic;
using System.Text;

namespace Slalom_To_Do_Application.BusinessProcessingLayer
{
    public class ProcessData
    {
        public ProcessData()
        {

        }

        public string processUserData (IEnumerable<UserEntity> userEntityData)
        {
            var _userEntityData = userEntityData;
            StringBuilder sb = new StringBuilder();

            sb.Append("[").AppendLine();
            foreach (var userEntity in _userEntityData)
            {
                sb.Append("{").AppendLine();
                sb.Append("\"User ID\":  ").Append("\"").Append(userEntity.user_id).Append("\",").AppendLine();
                sb.Append("\"First Name\":  ").Append("\"").Append(userEntity.user_first_name).Append("\",").AppendLine();
                sb.Append("\"Last Name\":  ").Append("\"").Append(userEntity.user_last_name).Append("\",").AppendLine();
                sb.Append("\"Creation Date\":  ").Append("\"").Append(userEntity.create_date).Append("\",").AppendLine();
                sb.Append("\"Modified Date\":  ").Append("\"").Append(userEntity.modified_date).Append("\",").AppendLine();
                sb.Append('}');

            }
            sb.Append("]").AppendLine();
            return sb.ToString();
        }
    }
}
